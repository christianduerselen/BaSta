using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace BaSta.Scoreboard
{
    public class DataBaseFunctions
    {
        private readonly string _userId;
        private readonly string _serverName;
        private readonly string _databaseName;
        private readonly string _password;

        public DataBaseFunctions(string serverName, string databaseName, string userid, string password)
        {
            _userId = userid;
            _serverName = serverName;
            _databaseName = databaseName;
            _password = password;
        }

        public SqlConnection Connection { get; private set; }

        public bool ConnectDatabase()
        {
            if (Connection != null)
            {
                if (Connection.State != ConnectionState.Closed)
                    Connection.Close();
            
                Connection.Dispose();
            }

            Connection = new SqlConnection("Data Source=" + _serverName + ";Initial Catalog=" + _databaseName + ";User ID = " + _userId + "; Password = " + _password);
            bool flag = false;
            try
            {
                Connection.Open();
                flag = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return flag;
        }
        
        public Team ReadTeam(int teamId)
        {
            Team team = null;
            SqlDataReader sqlDataReader = new SqlCommand($"SELECT * FROM Teams WHERE TeamID = {teamId}", Connection).ExecuteReader();
            while (sqlDataReader.Read())
            {
                team = new Team();
                team.TeamID = Convert.ToInt32(sqlDataReader[nameof(teamId)]);
                team.TeamName = (string) sqlDataReader["TeamName"];
                try
                {
                    team.TeamLogo = (byte[])sqlDataReader["TeamLogo"];
                }
                catch
                {
                    // Ignored
                }
            }
            sqlDataReader.Close();
            return team;
        }

        public int SaveTeam(Team team)
        {
            if (team.TeamLogo == null)
                team.TeamLogo = new byte[0];

            int num = -1;

            try
            {
                SqlCommand command = Connection.CreateCommand();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
                DataSet dataSet = new DataSet();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = nameof(SaveTeam);
                SqlParameter sqlParameter = new SqlParameter("@ID", SqlDbType.Int);
                sqlParameter.Direction = ParameterDirection.Output;
                command.Parameters.Add(sqlParameter);
                command.Parameters.Add("@TeamID", SqlDbType.Int).Value = team.TeamID;
                command.Parameters.Add("@TeamGameKindID", SqlDbType.Int).Value = team.TeamGameKindID;
                command.Parameters.Add("@TeamName", SqlDbType.NChar).Value = team.TeamName;
                command.Parameters.Add("@TeamLogo", SqlDbType.Image).Value = team.TeamLogo;
                try
                {
                    sqlDataAdapter.Fill(dataSet, "ID");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                num = Convert.ToInt32(sqlParameter.Value);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return num;
        }

        public List<Player> ReadCoaches(int teamId)
        {
            List<Player> playerList = new List<Player>();
            DataTable dataTable = new DataTable("Players");

            if (Connection.State == ConnectionState.Open)
            {
                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Player WHERE PlayerTeamID = " + teamId + " AND PlayerNumber < 0 ORDER BY PlayerNumber DESC", Connection);
                SqlDataReader sqlDataReader = null;
                try
                {
                    sqlDataReader = sqlCommand.ExecuteReader();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                dataTable.Columns.Add("PlayerTeamID");
                dataTable.Columns.Add("PlayerID");
                dataTable.Columns.Add("PlayerNumber");
                dataTable.Columns.Add("PlayerName");
                dataTable.Columns.Add("PlayerIsActive");
                dataTable.Load(sqlDataReader);
                sqlDataReader.Close();
            }

            if (dataTable.Rows.Count > 0)
            {
                for (int index = 0; index < dataTable.Rows.Count; ++index)
                    playerList.Add(new Player()
                    {
                        PlayerTeamID = Convert.ToInt32(dataTable.Rows[index].ItemArray[0]),
                        PlayerID = Convert.ToInt32(dataTable.Rows[index].ItemArray[1]),
                        PlayerNumber = Convert.ToInt32(dataTable.Rows[index].ItemArray[2]),
                        PlayerName = dataTable.Rows[index].ItemArray[3].ToString(),
                        PlayerIsActive = Convert.ToBoolean(dataTable.Rows[index].ItemArray[4])
                    });
            }

            return playerList;
        }

        public Player[] ReadPlayers(int TeamID)
        {
            Player[] playerArray = new Player[0];
            DataTable dataTable = new DataTable("Players");
            if (Connection.State == ConnectionState.Open)
            {
                SqlDataReader sqlDataReader = new SqlCommand("SELECT * FROM Player WHERE PlayerTeamID = " + TeamID.ToString() + " ORDER BY PlayerNumber", Connection).ExecuteReader();
                dataTable.Columns.Add("PlayerTeamID");
                dataTable.Columns.Add("PlayerID");
                dataTable.Columns.Add("PlayerNumber");
                dataTable.Columns.Add("PlayerName");
                dataTable.Columns.Add("PlayerIsActive");
                dataTable.Load((IDataReader)sqlDataReader);
                sqlDataReader.Close();
            }
            if (dataTable.Rows.Count > 0)
            {
                playerArray = new Player[dataTable.Rows.Count];
                for (int index = 0; index < dataTable.Rows.Count; ++index)
                {
                    playerArray[index] = new Player();
                    playerArray[index].PlayerTeamID = Convert.ToInt32(dataTable.Rows[index].ItemArray[0]);
                    playerArray[index].PlayerID = Convert.ToInt32(dataTable.Rows[index].ItemArray[1]);
                    playerArray[index].PlayerNumber = Convert.ToInt32(dataTable.Rows[index].ItemArray[2]);
                    playerArray[index].PlayerName = dataTable.Rows[index].ItemArray[3].ToString();
                    playerArray[index].PlayerIsActive = Convert.ToBoolean(dataTable.Rows[index].ItemArray[4]);
                }
            }
            return playerArray;
        }

        public void DeletePlayer(Player PlayerToBeDeleted)
        {
            new SqlCommand("DELETE FROM Player WHERE PlayerID = " + PlayerToBeDeleted.PlayerID.ToString(), Connection).ExecuteNonQuery();
        }

        public void SavePlayers(Player[] PlayersToBeSaved)
        {
            for (int index = 0; index < PlayersToBeSaved.Length; ++index)
                SavePlayer(PlayersToBeSaved[index]);
        }

        public int SavePlayer(Player PlayerToBeSaved)
        {
            SqlCommand command = Connection.CreateCommand();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
            DataSet dataSet = new DataSet();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = nameof(SavePlayer);
            command.Parameters.Add("@PlayerID", SqlDbType.Int).Value = (object)PlayerToBeSaved.PlayerID;
            command.Parameters.Add("@PlayerTeamID", SqlDbType.Int).Value = (object)PlayerToBeSaved.PlayerTeamID;
            command.Parameters.Add("@PlayerNumber", SqlDbType.Int).Value = (object)PlayerToBeSaved.PlayerNumber;
            command.Parameters.Add("@PlayerName", SqlDbType.NChar).Value = (object)PlayerToBeSaved.PlayerName.ToUpper();
            if (PlayerToBeSaved.PlayerIsActive)
                command.Parameters.Add("@PlayerIsActive", SqlDbType.Bit).Value = (object)1;
            else
                command.Parameters.Add("@PlayerIsActive", SqlDbType.Bit).Value = (object)0;
            SqlParameter sqlParameter = new SqlParameter("@ID", SqlDbType.Int);
            sqlParameter.Direction = ParameterDirection.Output;
            command.Parameters.Add(sqlParameter);
            try
            {
                sqlDataAdapter.Fill(dataSet, "ID");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Convert.ToInt32(sqlParameter.Value);
        }

        public Bitmap GamePlayerImage(int TeamID, int PlayerNumber)
        {
            Bitmap bitmap = (Bitmap)null;
            if (Connection.State == ConnectionState.Open)
            {
                SqlDataReader sqlDataReader = new SqlCommand("SELECT PlayerImage FROM Player WHERE PlayerTeamID = " + TeamID.ToString() + " AND PlayerNumber = " + PlayerNumber.ToString(), Connection).ExecuteReader();
                while (sqlDataReader.Read())
                {
                    try
                    {
                        byte[] buffer = (byte[])sqlDataReader["PlayerImage"];
                        MemoryStream memoryStream = new MemoryStream(buffer, 0, buffer.Length);
                        bitmap = new Bitmap((Stream)memoryStream);
                        memoryStream.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                sqlDataReader.Close();
            }
            return bitmap;
        }

        public Bitmap GamePlayerImage(int TeamID, int PlayerNumber, string PlayerName)
        {
            Bitmap bitmap = (Bitmap)null;
            if (Connection.State == ConnectionState.Open)
            {
                SqlDataReader sqlDataReader = new SqlCommand("SELECT PlayerImage FROM Player WHERE PlayerTeamID = " + TeamID.ToString() + " AND PlayerNumber = " + PlayerNumber.ToString() + " AND PlayerName = '" + PlayerName + "'", Connection).ExecuteReader();
                while (sqlDataReader.Read())
                {
                    try
                    {
                        byte[] buffer = (byte[])sqlDataReader["PlayerImage"];
                        MemoryStream memoryStream = new MemoryStream(buffer, 0, buffer.Length);
                        bitmap = new Bitmap((Stream)memoryStream);
                        memoryStream.Close();
                    }
                    catch
                    {
                    }
                }
                sqlDataReader.Close();
            }
            return bitmap;
        }

        public List<GamePlayer> GamePlayers(int GameID, int TeamID, bool IsCoach)
        {
            List<GamePlayer> gamePlayerList = new List<GamePlayer>();
            DataTable dataTable = new DataTable(nameof(GamePlayers));
            if (Connection.State == ConnectionState.Open)
            {
                string cmdText = "SELECT  * FROM GamePlayerData WHERE GamePlayerGameID = " + GameID.ToString() + " AND GamePlayerTeamID = " + TeamID.ToString() + " ORDER BY GamePlayerIsReservePlayer, GamePlayerIsCoach DESC, GamePlayerIsGoalkeeper DESC, GamePlayerPlayerNumber";
                if (IsCoach)
                    cmdText = "SELECT  * FROM GamePlayerData WHERE GamePlayerTeamID = " + TeamID.ToString() + " AND GamePlayerPlayerNumber < 0 ORDER BY GamePlayerGameID, GamePlayerPlayerNumber";
                SqlDataReader sqlDataReader = new SqlCommand(cmdText, Connection).ExecuteReader();
                dataTable.Columns.Add("GamePlayerPlayerNumber");
                dataTable.Columns.Add("GamePlayerName");
                dataTable.Columns.Add("GamePlayerPoints");
                dataTable.Columns.Add("GamePlayerFouls");
                dataTable.Columns.Add("GamePlayerIsReservePlayer");
                dataTable.Columns.Add("GamePlayerCards");
                dataTable.Columns.Add("GamePlayerIsGoalkeeper");
                dataTable.Load((IDataReader)sqlDataReader);
                sqlDataReader.Close();
            }
            if (dataTable.Rows.Count > 0)
            {
                for (int index = 0; index < dataTable.Rows.Count; ++index)
                {
                    GamePlayer gamePlayer = new GamePlayer();
                    gamePlayer.GameID = GameID;
                    gamePlayer.TeamID = TeamID;
                    gamePlayer.PlayerNumber = Convert.ToInt32(dataTable.Rows[index].ItemArray[0]);
                    gamePlayer.Name = dataTable.Rows[index].ItemArray[1].ToString();
                    gamePlayer.Points = Convert.ToInt32(dataTable.Rows[index].ItemArray[2]);
                    gamePlayer.Fouls = Convert.ToInt32(dataTable.Rows[index].ItemArray[3]);
                    gamePlayer.IsReservePlayer = Convert.ToBoolean(dataTable.Rows[index].ItemArray[4]);
                    try
                    {
                        gamePlayer.GamePlayerCards = Convert.ToInt32(dataTable.Rows[index].ItemArray[5]);
                    }
                    catch
                    {
                        gamePlayer.GamePlayerCards = 0;
                    }
                    try
                    {
                        gamePlayer.IsGoalkeeper = Convert.ToBoolean(dataTable.Rows[index].ItemArray[6]);
                    }
                    catch
                    {
                    }
                    gamePlayerList.Add(gamePlayer);
                }
            }
            return gamePlayerList;
        }

        public List<GamePlayer> GamePlayers(int GameID, int TeamID)
        {
            List<GamePlayer> gamePlayerList = new List<GamePlayer>();
            DataTable dataTable = new DataTable(nameof(GamePlayers));
            if (Connection.State == ConnectionState.Open)
            {
                SqlDataReader sqlDataReader = new SqlCommand("SELECT  * FROM GamePlayerData WHERE GamePlayerGameID = " + GameID.ToString() + " AND GamePlayerTeamID = " + TeamID.ToString() + " ORDER BY GamePlayerIsReservePlayer, GamePlayerIsCoach DESC, GamePlayerIsGoalkeeper DESC, GamePlayerPlayerNumber", Connection).ExecuteReader();
                dataTable.Columns.Add("GamePlayerPlayerNumber");
                dataTable.Columns.Add("GamePlayerName");
                dataTable.Columns.Add("GamePlayerPoints");
                dataTable.Columns.Add("GamePlayerFouls");
                dataTable.Columns.Add("GamePlayerIsReservePlayer");
                dataTable.Columns.Add("GamePlayerCards");
                dataTable.Columns.Add("GamePlayerIsGoalkeeper");
                dataTable.Load((IDataReader)sqlDataReader);
                sqlDataReader.Close();
            }
            if (dataTable.Rows.Count > 0)
            {
                for (int index = 0; index < dataTable.Rows.Count; ++index)
                {
                    GamePlayer gamePlayer = new GamePlayer();
                    gamePlayer.GameID = GameID;
                    gamePlayer.TeamID = TeamID;
                    gamePlayer.PlayerNumber = Convert.ToInt32(dataTable.Rows[index].ItemArray[0]);
                    gamePlayer.Name = dataTable.Rows[index].ItemArray[1].ToString();
                    gamePlayer.Points = Convert.ToInt32(dataTable.Rows[index].ItemArray[2]);
                    gamePlayer.Fouls = Convert.ToInt32(dataTable.Rows[index].ItemArray[3]);
                    gamePlayer.IsReservePlayer = Convert.ToBoolean(dataTable.Rows[index].ItemArray[4]);
                    try
                    {
                        gamePlayer.GamePlayerCards = Convert.ToInt32(dataTable.Rows[index].ItemArray[5]);
                    }
                    catch
                    {
                    }
                    try
                    {
                        gamePlayer.IsGoalkeeper = Convert.ToBoolean(dataTable.Rows[index].ItemArray[6]);
                    }
                    catch
                    {
                    }
                    gamePlayerList.Add(gamePlayer);
                }
            }
            return gamePlayerList;
        }

        public List<GamePlayer> ReadGamePlayers(int GameID, int TeamID)
        {
            List<GamePlayer> gamePlayerList = new List<GamePlayer>();
            DataTable dataTable = new DataTable("GamePlayers");
            if (Connection.State == ConnectionState.Open)
            {
                SqlDataReader sqlDataReader = new SqlCommand("SELECT  * FROM GamePlayerData WHERE GamePlayerGameID = " + GameID.ToString() + " AND GamePlayerTeamID = " + TeamID.ToString() + " ORDER BY GamePlayerGameID, GamePlayerIsCoach DESC, GamePlayerIsGoalkeeper DESC, GamePlayerPlayerNumber", Connection).ExecuteReader();
                dataTable.Columns.Add("GamePlayerPlayerNumber");
                dataTable.Columns.Add("GamePlayerName");
                dataTable.Columns.Add("GamePlayerPoints");
                dataTable.Columns.Add("GamePlayerFouls");
                dataTable.Columns.Add("GamePlayerIsReservePlayer");
                dataTable.Columns.Add("GamePlayerCards");
                dataTable.Columns.Add("GamePlayerIsGoalkeeper");
                dataTable.Columns.Add("GamePlayerIsCoach");
                dataTable.Load((IDataReader)sqlDataReader);
                sqlDataReader.Close();
            }
            if (dataTable.Rows.Count > 0)
            {
                for (int index = 0; index < dataTable.Rows.Count; ++index)
                {
                    GamePlayer gamePlayer = new GamePlayer();
                    gamePlayer.GameID = GameID;
                    gamePlayer.TeamID = TeamID;
                    gamePlayer.PlayerNumber = Convert.ToInt32(dataTable.Rows[index].ItemArray[0]);
                    gamePlayer.Name = dataTable.Rows[index].ItemArray[1].ToString();
                    gamePlayer.Points = Convert.ToInt32(dataTable.Rows[index].ItemArray[2]);
                    gamePlayer.Fouls = Convert.ToInt32(dataTable.Rows[index].ItemArray[3]);
                    gamePlayer.IsReservePlayer = Convert.ToBoolean(dataTable.Rows[index].ItemArray[4]);
                    gamePlayer.GamePlayerCards = Convert.ToInt32(dataTable.Rows[index].ItemArray[5]);
                    try
                    {
                        gamePlayer.IsGoalkeeper = Convert.ToBoolean(dataTable.Rows[index].ItemArray[6]);
                    }
                    catch
                    {
                    }
                    try
                    {
                        gamePlayer.IsCoach = Convert.ToBoolean(dataTable.Rows[index].ItemArray[7]);
                    }
                    catch
                    {
                    }
                    gamePlayerList.Add(gamePlayer);
                }
            }
            return gamePlayerList;
        }

        public void SaveGamePlayers(ArrayList PlayersToBeSaved)
        {
            for (int index = 0; index < PlayersToBeSaved.Count; ++index)
                SaveGamePlayer((GamePlayer)PlayersToBeSaved[index]);
        }

        public void SaveGamePlayers(GamePlayer[] PlayersToBeSaved)
        {
            for (int index = 0; index < PlayersToBeSaved.Length; ++index)
                SaveGamePlayer(PlayersToBeSaved[index]);
        }

        public void SaveGamePlayer(GamePlayer PlayerToBeSaved)
        {
            SqlCommand command = Connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = nameof(SaveGamePlayer);
            command.Parameters.Add("@GamePlayerGameID", SqlDbType.Int).Value = (object)PlayerToBeSaved.GameID;
            command.Parameters.Add("@GamePlayerTeamID", SqlDbType.Int).Value = (object)PlayerToBeSaved.TeamID;
            command.Parameters.Add("@GamePlayerPlayerNumber", SqlDbType.Int).Value = (object)PlayerToBeSaved.PlayerNumber;
            command.Parameters.Add("@GamePlayerName", SqlDbType.NChar).Value = (object)PlayerToBeSaved.Name;
            command.Parameters.Add("@GamePlayerPoints", SqlDbType.Int).Value = (object)PlayerToBeSaved.Points;
            command.Parameters.Add("@GamePlayerFouls", SqlDbType.Int).Value = (object)PlayerToBeSaved.Fouls;
            command.ExecuteNonQuery();
        }

        public void DeleteGamePlayers(int GameID, int TeamID)
        {
            new SqlCommand("DELETE FROM GamePlayerData WHERE GamePlayerGameID = " + GameID.ToString() + " AND GamePlayerTeamID = " + TeamID.ToString(), Connection).ExecuteNonQuery();
        }

        public void DeleteGamePlayer(GamePlayer PlayerToBeDeleted)
        {
            new SqlCommand("DELETE FROM GamePlayerData WHERE GamePlayerGameID = " + PlayerToBeDeleted.GameID.ToString() + " AND GamePlayerTeamID = " + PlayerToBeDeleted.TeamID.ToString() + " AND GamePlayerPlayerNumber = " + PlayerToBeDeleted.PlayerNumber.ToString(), Connection).ExecuteNonQuery();
        }

        public void ShowGamePlayers(Form Target, ArrayList GamePlayerHome, ArrayList GamePlayerGuest)
        {
            for (int index = 0; index < 15; ++index)
            {
                if (index < GamePlayerHome.Count)
                {
                    GamePlayer gamePlayer = (GamePlayer)GamePlayerHome[index];
                    Target.Controls["FoulNoH" + (index + 1).ToString()].Text = gamePlayer.PlayerNumber.ToString();
                    Target.Controls["NameH" + (index + 1).ToString()].Text = gamePlayer.Name.Trim();
                    Target.Controls["PointsH" + (index + 1).ToString()].Text = gamePlayer.Points.ToString();
                    Target.Controls["FoulsH" + (index + 1).ToString()].Text = gamePlayer.Fouls.ToString();
                }
                else
                {
                    Target.Controls["FoulNoH" + (index + 1).ToString()].Text = string.Empty;
                    Target.Controls["NameH" + (index + 1).ToString()].Text = string.Empty;
                    Target.Controls["PointsH" + (index + 1).ToString()].Text = string.Empty;
                    Target.Controls["FoulsH" + (index + 1).ToString()].Text = string.Empty;
                }
                if (index < GamePlayerGuest.Count)
                {
                    GamePlayer gamePlayer = (GamePlayer)GamePlayerGuest[index];
                    Target.Controls["FoulNoG" + (index + 1).ToString()].Text = gamePlayer.PlayerNumber.ToString();
                    Target.Controls["NameG" + (index + 1).ToString()].Text = gamePlayer.Name.Trim();
                    Target.Controls["PointsG" + (index + 1).ToString()].Text = gamePlayer.Points.ToString();
                    Target.Controls["FoulsG" + (index + 1).ToString()].Text = gamePlayer.Fouls.ToString();
                }
                else
                {
                    Target.Controls["FoulNoG" + (index + 1).ToString()].Text = string.Empty;
                    Target.Controls["NameG" + (index + 1).ToString()].Text = string.Empty;
                    Target.Controls["PointsG" + (index + 1).ToString()].Text = string.Empty;
                    Target.Controls["FoulsG" + (index + 1).ToString()].Text = string.Empty;
                }
            }
        }

        public byte[] ImageToByteArray(Image Picture)
        {
            Bitmap bitmap = new Bitmap(Picture);
            MemoryStream memoryStream = new MemoryStream();
            bitmap.Save((Stream)memoryStream, ImageFormat.Jpeg);
            return memoryStream.ToArray();
        }

        public Image ByteArrayToImage(byte[] _array)
        {
            return (Image)new Bitmap((Stream)new MemoryStream(_array, 0, _array.Length));
        }

        public delegate void PercentCompleteEventHandler(DataBaseFunctions sender, int percent);
    }
}
