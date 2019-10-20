using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace BaSta.Game
{
    public class DataBaseFunctions
    {
        private string _user_id = string.Empty;
        private string _servername = string.Empty;
        private string _databasename = string.Empty;
        private string _password = string.Empty;
        private SqlConnection _conn;

        public DataBaseFunctions(
          string servername,
          string databasename,
          string userid,
          string password)
        {
            _user_id = userid;
            _servername = servername;
            _databasename = databasename;
            _password = password;
        }

        public string UserID
        {
            get
            {
                return _user_id;
            }
            set
            {
                if (!(_user_id != value))
                    return;
                _user_id = value;
                ConnectDatabase();
            }
        }

        public string ServerName
        {
            get
            {
                return _servername;
            }
            set
            {
                if (!(_servername != value))
                    return;
                _servername = value;
                ConnectDatabase();
            }
        }

        public string DatabaseName
        {
            get
            {
                return _databasename;
            }
            set
            {
                if (!(_databasename != value))
                    return;
                _databasename = value;
                ConnectDatabase();
            }
        }

        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                if (!(_password != value))
                    return;
                _password = value;
                ConnectDatabase();
            }
        }

        public SqlConnection Connection
        {
            get
            {
                return _conn;
            }
            set
            {
                _conn = value;
            }
        }

        public bool ConnectDatabase(string ServerName, string DatabaseName, string UserID)
        {

            _databasename = DatabaseName;
            _user_id = UserID;
            return ConnectDatabase();
        }

        public bool ConnectDatabase()
        {
            if (_conn != null)
            {
                if (_conn.State != ConnectionState.Closed)
                    _conn.Close();
                _conn.Dispose();
            }
            _servername = ".\\SQLEXPRESS";

            _conn = new SqlConnection("Data Source=" + _servername + ";Initial Catalog=" + _databasename + ";User ID = " + _user_id + "; Password = " + _password);
            bool flag = false;
            try
            {
                _conn.Open();
                flag = true;
            }
            catch (Exception)
            {
                // Ignored
            }

            return flag;
        }

        public bool DatabaseExists(string servername, string databasename)
        {
            return new Server(servername).Databases.Contains(databasename);
        }

        public void CreateDatabase(string servername, string databasename)
        {
            if (new Server(servername).Databases.Contains(databasename))
                return;
            string cmdText = "CREATE DATABASE " + databasename;
            SqlConnection connection = new SqlConnection("Data Source=" + servername + ";Initial Catalog=master;Integrated Security=True");
            connection.Open();
            new SqlCommand(cmdText, connection).ExecuteNonQuery();
        }

        public void CreateDatabaseTables(string servername, string databasename, string sourcepath)
        {
            foreach (FileSystemInfo file in new DirectoryInfo(sourcepath).GetFiles("*.tab"))
                CreateDatabaseTable(servername, databasename, file.Name);
        }

        public void CreateDatabaseTable(string servername, string databasename, string tablename)
        {
            if (new Server(servername).Databases[databasename].Tables.Contains(tablename.Substring(0, tablename.IndexOf('.'))))
                return;
            string cmdText = _read_sql_script(tablename);
            SqlConnection connection = new SqlConnection("Data Source=" + servername + ";Initial Catalog=" + databasename + ";Integrated Security=True");
            connection.Open();
            new SqlCommand(cmdText, connection).ExecuteNonQuery();
        }

        public void CreateDatabaseStoredProcedures(
          string servername,
          string databasename,
          string sourcepath)
        {
            foreach (FileSystemInfo file in new DirectoryInfo(sourcepath).GetFiles("*.sp"))
                CreateDatabaseStoredProcedure(servername, databasename, file.Name);
        }

        public void CreateDatabaseStoredProcedure(
          string servername,
          string databasename,
          string procedurename)
        {
            if (new Server(servername).Databases[databasename].StoredProcedures.Contains(procedurename.Substring(0, procedurename.IndexOf('.'))))
                return;
            string cmdText = _read_sql_script(procedurename);
            SqlConnection connection = new SqlConnection("Data Source=" + servername + ";Initial Catalog=" + databasename + ";Integrated Security=True");
            connection.Open();
            new SqlCommand(cmdText, connection).ExecuteNonQuery();
        }

        public void KeepConnectionAlive()
        {
            try
            {
                if (_conn.State == ConnectionState.Open)
                    new SqlCommand("SELECT * FROM GameKinds", _conn).ExecuteNonQuery();
                else
                    ConnectDatabase();
            }
            catch
            {
            }
        }

        private string _read_sql_script(string tablename)
        {
            string str1 = string.Empty;
            if (File.Exists(Application.StartupPath + "\\" + tablename))
            {
                StreamReader streamReader = new StreamReader(Application.StartupPath + "\\" + tablename);
                for (string str2 = streamReader.ReadLine(); str2 != null; str2 = streamReader.ReadLine())
                {
                    if (str2 != null)
                        str1 = str1 + str2.Replace(string.Concat((object)'\t'), string.Empty) + " ";
                }
                streamReader.Close();
            }
            return str1;
        }

        public event DataBaseFunctions.PercentCompleteEventHandler PercentComplete;

        public void CreateBackupToFile(string serverName, string databaseName, string backupFileName)
        {
            ServerConnection serverConnection = new ServerConnection();
            serverConnection.ServerInstance = serverName;
            serverConnection.LoginSecure = true;
            Server srv = new Server(serverConnection);
            try
            {
                srv.ConnectionContext.Connect();
                Backup backup = new Backup();
                backup.Action = BackupActionType.Files;
                backup.Database = databaseName;
                if (PercentComplete != null)
                    backup.PercentComplete += new Microsoft.SqlServer.Management.Smo.PercentCompleteEventHandler(backup_PercentComplete);
                backup.Devices.Add(new BackupDeviceItem(backupFileName, DeviceType.File));
                backup.SqlBackup(srv);
            }
            finally
            {
                try
                {
                    srv.ConnectionContext.Disconnect();
                }
                catch
                {
                }
            }
        }

        private void backup_PercentComplete(object sender, PercentCompleteEventArgs e)
        {
        }

        public void RestoreFromFile(string serverName, string databaseName, string backupFileName)
        {
            ServerConnection serverConnection = new ServerConnection();
            serverConnection.ServerInstance = serverName;
            serverConnection.LoginSecure = true;
            Server srv = new Server(serverConnection);
            try
            {
                srv.ConnectionContext.Connect();
                Restore restore = new Restore();
                restore.Action = RestoreActionType.Database;
                restore.Database = databaseName;
                if (PercentComplete != null)
                    restore.PercentComplete += new Microsoft.SqlServer.Management.Smo.PercentCompleteEventHandler(restore_PercentComplete);
                restore.Devices.Add(new BackupDeviceItem(backupFileName, DeviceType.File));
                restore.ReplaceDatabase = true;
                restore.SqlRestore(srv);
            }
            finally
            {
                try
                {
                    srv.ConnectionContext.Disconnect();
                }
                catch
                {
                }
            }
        }

        private void restore_PercentComplete(object sender, PercentCompleteEventArgs e)
        {
        }

        public void FillKindOfGames()
        {
            if (!File.Exists(Application.StartupPath + "\\GameKinds.cfg"))
                return;
            string cmdText = "DROP TABLE GameKinds";
            if (_conn.State != ConnectionState.Open)
                _conn.Open();
            SqlCommand sqlCommand1 = new SqlCommand(cmdText, _conn);
            try
            {
                sqlCommand1.ExecuteNonQuery();
            }
            catch
            {
            }
            try
            {
                CreateDatabaseTable(_servername, _databasename, "GameKinds.tab");
            }
            catch
            {
            }
            StreamReader streamReader = new StreamReader(Application.StartupPath + "\\GameKinds.cfg");
            for (string str = streamReader.ReadLine(); str != null; str = streamReader.ReadLine())
            {
                if (str != null)
                {
                    string[] strArray = str.Split(';');
                    SqlCommand sqlCommand2 = new SqlCommand("INSERT INTO GameKinds (GameName, GameNameAlias) VALUES ('" + strArray[0] + "', '" + strArray[1] + "')", _conn);
                    try
                    {
                        sqlCommand2.ExecuteNonQuery();
                    }
                    catch
                    {
                    }
                }
            }
            streamReader.Close();
        }

        public List<KindOfGame> ReadKindOfGames()
        {
            List<KindOfGame> kindOfGameList = new List<KindOfGame>();
            DataTable dataTable = new DataTable("KindOfGames");
            if (_conn.State == ConnectionState.Open)
            {
                SqlDataReader sqlDataReader = new SqlCommand("SELECT * FROM GameKinds ORDER BY ID", _conn).ExecuteReader();
                dataTable.Columns.Add("ID");
                dataTable.Columns.Add("GameName");
                dataTable.Columns.Add("GameNameAlias");
                dataTable.Load((IDataReader)sqlDataReader);
            }
            if (dataTable.Rows.Count > 0)
            {
                for (int index = 0; index < dataTable.Rows.Count; ++index)
                    kindOfGameList.Add(new KindOfGame(Convert.ToInt32(dataTable.Rows[index].ItemArray[0]), dataTable.Rows[index].ItemArray[1].ToString().Trim(), dataTable.Rows[index].ItemArray[2].ToString().Trim()));
            }
            return kindOfGameList;
        }

        public GameParameter[] GameParameter(int GameKindID, int Period)
        {
            GameParameter[] gameParameterArray = (GameParameter[])null;
            DataTable dataTable = new DataTable(nameof(GameParameter));
            if (_conn.State == ConnectionState.Open)
            {
                SqlDataReader sqlDataReader = new SqlCommand("SELECT * FROM GameStartParameter WHERE GamekindID = " + GameKindID.ToString() + " AND Period = " + Period.ToString(), _conn).ExecuteReader();
                dataTable.Columns.Add(nameof(GameKindID));
                dataTable.Columns.Add(nameof(Period));
                dataTable.Columns.Add("ParameterName");
                dataTable.Columns.Add("ParameterIntValue");
                dataTable.Load((IDataReader)sqlDataReader);
            }
            if (dataTable.Rows.Count > 0)
            {
                gameParameterArray = new GameParameter[dataTable.Rows.Count];
                for (int index = 0; index < dataTable.Rows.Count; ++index)
                    gameParameterArray[index] = new GameParameter(Convert.ToInt32(dataTable.Rows[index].ItemArray[0]), Convert.ToInt32(dataTable.Rows[index].ItemArray[1]), dataTable.Rows[index].ItemArray[2].ToString().Trim(), Convert.ToInt64(dataTable.Rows[index].ItemArray[3]));
            }
            return gameParameterArray;
        }

        public Hashtable GameParameterHashTable(int GameKindID, int Period)
        {
            Hashtable hashtable = (Hashtable)null;
            DataTable dataTable = new DataTable("GameParameter");
            if (_conn.State == ConnectionState.Open)
            {
                SqlDataReader sqlDataReader = new SqlCommand("SELECT * FROM GameStartParameter WHERE GamekindID = " + GameKindID.ToString() + " AND Period = " + Period.ToString(), _conn).ExecuteReader();
                dataTable.Columns.Add(nameof(GameKindID));
                dataTable.Columns.Add(nameof(Period));
                dataTable.Columns.Add("ParameterName");
                dataTable.Columns.Add("ParameterIntValue");
                dataTable.Load((IDataReader)sqlDataReader);
            }
            if (dataTable.Rows.Count > 0)
            {
                hashtable = new Hashtable();
                for (int index = 0; index < dataTable.Rows.Count; ++index)
                {
                    GameParameter gameParameter = new GameParameter(Convert.ToInt32(dataTable.Rows[index].ItemArray[0]), Convert.ToInt32(dataTable.Rows[index].ItemArray[1]), dataTable.Rows[index].ItemArray[2].ToString().Trim(), Convert.ToInt64(dataTable.Rows[index].ItemArray[3]));
                    hashtable.Add((object)gameParameter.ParameterName, (object)gameParameter);
                }
            }
            return hashtable;
        }

        public void WriteGameParameters(GameParameter[] Parameters)
        {
            for (int index = 0; index < Parameters.Length; ++index)
                WriteGameParameter(Parameters[index]);
        }

        public void WriteGameParameter(GameParameter Parameter)
        {
            SqlCommand command = _conn.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@GameKindID", SqlDbType.Int).Value = (object)Parameter.GameKindID;
            command.Parameters.Add("@Period", SqlDbType.Int).Value = (object)Parameter.Period;
            command.Parameters.Add("@ParameterName", SqlDbType.NChar).Value = (object)Parameter.ParameterName;
            command.CommandText = "SaveIntGameParameter";
            command.Parameters.Add("@ParameterIntValue", SqlDbType.Int).Value = (object)Parameter.ParameterIntValue;
            command.ExecuteNonQuery();
        }

        public void DeleteGameParameter(int GameKindID)
        {
            new SqlCommand("DELETE FROM GameStartParameter WHERE GameKindID = " + GameKindID.ToString(), _conn).ExecuteNonQuery();
        }

        public void DeleteGameParameter(int GameKindID, int Period)
        {
            new SqlCommand("DELETE FROM GameStartParameter WHERE GameKindID = " + GameKindID.ToString() + " AND Period = " + Period.ToString(), _conn).ExecuteNonQuery();
        }

        public long PointsToWin(int GameKindID, int Period)
        {
            long num = -1;
            Hashtable hashtable = GameParameterHashTable(GameKindID, Period);
            if (hashtable != null && hashtable.Contains((object)"PeriodEndsAtScore"))
                num = ((GameParameter)hashtable[(object)"PeriodEndsAtScore"]).ParameterIntValue;
            return num;
        }

        public long PointDiffToWin(int GameKindID, int Period)
        {
            long num = -1;
            Hashtable hashtable = GameParameterHashTable(GameKindID, Period);
            if (hashtable != null && hashtable.Contains((object)"PeriodEndMinScoreDiff"))
                num = ((GameParameter)hashtable[(object)"PeriodEndMinScoreDiff"]).ParameterIntValue;
            return num;
        }

        public bool CheckIfPeriodsComplete(int GameKindID, int DesiredPeriods)
        {
            int num = -1;
            SqlDataReader sqlDataReader = new SqlCommand("SELECT maxPeriod = MAX(Period) FROM GameStartParameter WHERE GameKindID = " + GameKindID.ToString(), _conn).ExecuteReader();
            try
            {
                while (sqlDataReader.Read())
                    num = Convert.ToInt32(sqlDataReader["maxPeriod"]);
            }
            catch
            {
                num = 0;
            }
            sqlDataReader.Close();
            return num >= DesiredPeriods;
        }

        public void DeleteUndesiredPeriods(int GameKindID, int DesiredPeriods)
        {
            new SqlCommand("DELETE FROM GameStartParameter WHERE GameKindID = " + GameKindID.ToString() + " AND Period > " + DesiredPeriods.ToString(), _conn).ExecuteNonQuery();
        }

        public PeriodParameter[] PeriodParameter(int GameKindID, int Period)
        {
            PeriodParameter[] periodParameterArray = (PeriodParameter[])null;
            DataTable dataTable = new DataTable("GameParameter");
            if (_conn.State == ConnectionState.Open)
            {
                SqlDataReader sqlDataReader = new SqlCommand("SELECT * FROM GameStartParameter WHERE GamekindID = " + GameKindID.ToString() + " AND Period = " + Period.ToString(), _conn).ExecuteReader();
                dataTable.Columns.Add("ParameterName");
                dataTable.Columns.Add("ParameterIntValue");
                dataTable.Load((IDataReader)sqlDataReader);
            }
            if (dataTable.Rows.Count > 0)
            {
                periodParameterArray = new PeriodParameter[dataTable.Rows.Count];
                for (int index = 0; index < dataTable.Rows.Count; ++index)
                    periodParameterArray[index] = new PeriodParameter(dataTable.Rows[index].ItemArray[0].ToString().Trim(), Convert.ToInt64(dataTable.Rows[index].ItemArray[1]));
            }
            return periodParameterArray;
        }

        public Dictionary<string, PeriodParameter> PeriodParameterDictionary(
          int GameKindID,
          int Period)
        {
            Dictionary<string, PeriodParameter> dictionary = (Dictionary<string, PeriodParameter>)null;
            DataTable dataTable = new DataTable("GameParameter");
            if (_conn.State == ConnectionState.Open)
            {
                SqlDataReader sqlDataReader = new SqlCommand("SELECT * FROM GameStartParameter WHERE GamekindID = " + GameKindID.ToString() + " AND Period = " + Period.ToString(), _conn).ExecuteReader();
                dataTable.Columns.Add("ParameterName");
                dataTable.Columns.Add("ParameterIntValue");
                dataTable.Load((IDataReader)sqlDataReader);
            }
            if (dataTable.Rows.Count > 0)
            {
                dictionary = new Dictionary<string, PeriodParameter>();
                for (int index = 0; index < dataTable.Rows.Count; ++index)
                {
                    PeriodParameter periodParameter = new PeriodParameter(dataTable.Rows[index].ItemArray[0].ToString().Trim(), Convert.ToInt64(dataTable.Rows[index].ItemArray[1]));
                    dictionary.Add(periodParameter.ParameterName, periodParameter);
                }
            }
            return dictionary;
        }

        public Team ReadTeam(int GameKindID, string TeamName)
        {
            Team team = (Team)null;
            SqlDataReader sqlDataReader = new SqlCommand("SELECT * FROM Teams WHERE TeamName = '" + TeamName + "' AND TeamGamekindID = " + GameKindID.ToString(), _conn).ExecuteReader();
            while (sqlDataReader.Read())
            {
                team = new Team();
                team.TeamID = Convert.ToInt32(sqlDataReader["TeamID"]);
                team.TeamName = (string)sqlDataReader[nameof(TeamName)];
                try
                {
                    team.TeamLogo = (byte[])sqlDataReader["TeamLogo"];
                }
                catch
                {
                }
            }
            sqlDataReader.Close();
            return team;
        }

        public Team ReadTeam(int TeamID)
        {
            Team team = (Team)null;
            SqlDataReader sqlDataReader = new SqlCommand("SELECT * FROM Teams WHERE TeamID = " + TeamID.ToString(), _conn).ExecuteReader();
            while (sqlDataReader.Read())
            {
                team = new Team();
                team.TeamID = Convert.ToInt32(sqlDataReader[nameof(TeamID)]);
                team.TeamName = (string)sqlDataReader["TeamName"];
                try
                {
                    team.TeamLogo = (byte[])sqlDataReader["TeamLogo"];
                }
                catch
                {
                }
            }
            sqlDataReader.Close();
            return team;
        }

        public List<Team> ReadTeams(int GameKindID)
        {
            int num1 = GameKindID;
            List<Team> teamList = (List<Team>)null;
            if (_conn.State == ConnectionState.Open && num1 > -1)
            {
                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Teams WHERE TeamGameKindID = " + num1.ToString() + " AND NOT TeamName = 'Referees' ORDER BY TeamName", _conn);
                SqlDataReader sqlDataReader1 = sqlCommand.ExecuteReader();
                int num2 = 0;
                while (sqlDataReader1.Read())
                    ++num2;
                sqlDataReader1.Close();
                SqlDataReader sqlDataReader2 = sqlCommand.ExecuteReader();
                teamList = new List<Team>();
                while (sqlDataReader2.Read())
                {
                    Team team = new Team();
                    team.TeamID = Convert.ToInt32(sqlDataReader2["TeamID"]);
                    team.TeamName = (string)sqlDataReader2["TeamName"];
                    try
                    {
                        team.TeamLogo = (byte[])sqlDataReader2["TeamLogo"];
                    }
                    catch
                    {
                    }
                    teamList.Add(team);
                }
                sqlDataReader2.Close();
            }
            return teamList;
        }

        public int SaveTeam(Team ThisTeam)
        {
            int num = -1;
            if (ThisTeam.TeamLogo == null)
                ThisTeam.TeamLogo = new byte[0];
            try
            {
                SqlCommand command = _conn.CreateCommand();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
                DataSet dataSet = new DataSet();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = nameof(SaveTeam);
                SqlParameter sqlParameter = new SqlParameter("@ID", SqlDbType.Int);
                sqlParameter.Direction = ParameterDirection.Output;
                command.Parameters.Add(sqlParameter);
                command.Parameters.Add("@TeamID", SqlDbType.Int).Value = (object)ThisTeam.TeamID;
                command.Parameters.Add("@TeamGameKindID", SqlDbType.Int).Value = (object)ThisTeam.TeamGameKindID;
                command.Parameters.Add("@TeamName", SqlDbType.NChar).Value = (object)ThisTeam.TeamName;
                command.Parameters.Add("@TeamLogo", SqlDbType.Image).Value = (object)ThisTeam.TeamLogo;
                try
                {
                    sqlDataAdapter.Fill(dataSet, "ID");
                }
                catch
                {
                }
                num = Convert.ToInt32(sqlParameter.Value);
            }
            catch
            {
            }
            return num;
        }

        public void SaveTeamLogo(int teamID, string filePath)
        {
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader((Stream)fileStream);
            byte[] numArray = binaryReader.ReadBytes((int)fileStream.Length);
            binaryReader.Close();
            fileStream.Close();
            SqlCommand sqlCommand = new SqlCommand("UPDATE TEAMS SET TeamLogo =(@datei) WHERE TeamID = " + teamID.ToString(), _conn);
            sqlCommand.Parameters.Add("@datei", SqlDbType.VarBinary).Value = (object)numArray;
            try
            {
                sqlCommand.ExecuteNonQuery();
            }
            catch
            {
            }
        }

        public void DeleteTeam(int ID)
        {
            new SqlCommand("DELETE FROM Teams WHERE TeamID = " + ID.ToString(), _conn).ExecuteNonQuery();
            new SqlCommand("DELETE FROM Player WHERE PlayerTeamID = " + ID.ToString(), _conn).ExecuteNonQuery();
            new SqlCommand("DELETE FROM GamePlayerData WHERE GamePlayerTeamID = " + ID.ToString(), _conn).ExecuteNonQuery();
        }

        public List<Player> ReadPlayers(int TeamID, bool CoachesOnly)
        {
            List<Player> playerList = new List<Player>();
            DataTable dataTable = new DataTable("Players");
            if (_conn.State == ConnectionState.Open)
            {
                try
                {
                    string str = "SELECT * FROM Player WHERE PlayerTeamID = " + TeamID.ToString();
                    SqlDataReader sqlDataReader = new SqlCommand(!CoachesOnly ? str + " AND PlayerNumber > -1 ORDER BY PlayerIsCoach DESC, PlayerIsGoalkeeper DESC, PlayerNumber" : str + " AND PlayerNumber < 0 ORDER BY PlayerNumber", _conn).ExecuteReader();
                    dataTable.Columns.Add("PlayerTeamID");
                    dataTable.Columns.Add("PlayerID");
                    dataTable.Columns.Add("PlayerNumber");
                    dataTable.Columns.Add("PlayerName");
                    dataTable.Columns.Add("PlayerIsActive");
                    dataTable.Columns.Add("PlayerIsReservePlayer");
                    dataTable.Columns.Add("PlayerIsGoalkeeper");
                    dataTable.Columns.Add("PlayerIsCoach");
                    dataTable.Load((IDataReader)sqlDataReader);
                    sqlDataReader.Close();
                }
                catch
                {
                    string str = "SELECT * FROM Player WHERE PlayerTeamID = " + TeamID.ToString();
                    SqlDataReader sqlDataReader = new SqlCommand(!CoachesOnly ? str + " AND PlayerNumber > -1 ORDER BY PlayerNumber" : str + " AND PlayerNumber < 0 ORDER BY PlayerNumber", _conn).ExecuteReader();
                    dataTable.Columns.Add("PlayerTeamID");
                    dataTable.Columns.Add("PlayerID");
                    dataTable.Columns.Add("PlayerNumber");
                    dataTable.Columns.Add("PlayerName");
                    dataTable.Columns.Add("PlayerIsActive");
                    dataTable.Columns.Add("PlayerIsReservePlayer");
                    dataTable.Load((IDataReader)sqlDataReader);
                    sqlDataReader.Close();
                }
            }
            if (dataTable.Rows.Count > 0)
            {
                for (int index = 0; index < dataTable.Rows.Count; ++index)
                {
                    Player player = new Player();
                    player.PlayerTeamID = Convert.ToInt32(dataTable.Rows[index].ItemArray[0]);
                    player.PlayerID = Convert.ToInt32(dataTable.Rows[index].ItemArray[1]);
                    player.PlayerNumber = Convert.ToInt32(dataTable.Rows[index].ItemArray[2]);
                    player.PlayerName = dataTable.Rows[index].ItemArray[3].ToString();
                    player.PlayerIsActive = Convert.ToBoolean(dataTable.Rows[index].ItemArray[4]);
                    try
                    {
                        player.PlayerIsReservePlayer = Convert.ToBoolean(dataTable.Rows[index].ItemArray[5]);
                    }
                    catch
                    {
                    }
                    try
                    {
                        player.PlayerIsGoalkeeper = Convert.ToBoolean(dataTable.Rows[index].ItemArray[6]);
                    }
                    catch
                    {
                    }
                    try
                    {
                        player.PlayerIsCoach = Convert.ToBoolean(dataTable.Rows[index].ItemArray[7]);
                    }
                    catch
                    {
                    }
                    playerList.Add(player);
                }
            }
            return playerList;
        }

        public List<Player> ReadCoaches(int TeamID)
        {
            List<Player> playerList = new List<Player>();
            DataTable dataTable = new DataTable("Coaches");
            if (_conn.State == ConnectionState.Open)
            {
                SqlDataReader sqlDataReader = new SqlCommand("SELECT * FROM Player WHERE PlayerTeamID = " + TeamID.ToString() + " AND PlayerNumber < 0 ORDER BY PlayerNumber", _conn).ExecuteReader();
                dataTable.Columns.Add("PlayerTeamID");
                dataTable.Columns.Add("PlayerID");
                dataTable.Columns.Add("PlayerNumber");
                dataTable.Columns.Add("PlayerName");
                dataTable.Load((IDataReader)sqlDataReader);
                sqlDataReader.Close();
            }
            if (dataTable.Rows.Count > 0)
            {
                playerList = new List<Player>();
                for (int index = 0; index < dataTable.Rows.Count; ++index)
                {
                    playerList.Add(new Player());
                    playerList[index].PlayerTeamID = Convert.ToInt32(dataTable.Rows[index].ItemArray[0]);
                    playerList[index].PlayerID = Convert.ToInt32(dataTable.Rows[index].ItemArray[1]);
                    playerList[index].PlayerNumber = Convert.ToInt32(dataTable.Rows[index].ItemArray[2]);
                    playerList[index].PlayerName = dataTable.Rows[index].ItemArray[3].ToString();
                    playerList[index].PlayerIsActive = true;
                    playerList[index].PlayerIsReservePlayer = false;
                }
            }
            return playerList;
        }

        public List<Player> ReadPlayersWithImage(int TeamID)
        {
            List<Player> playerList = new List<Player>();
            DataTable dataTable = new DataTable("Players");
            if (_conn.State == ConnectionState.Open && TeamID > -1)
            {
                string cmdText = "SELECT * FROM Player WHERE PlayerTeamID = " + TeamID.ToString() + " ORDER BY PlayerIsCoach DESC, PlayerIsGoalkeeper DESC, PlayerNumber";
                try
                {
                    SqlCommand sqlCommand = new SqlCommand(cmdText, _conn);
                    sqlCommand.ExecuteReader().Close();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    while (sqlDataReader.Read())
                    {
                        Player player = new Player();
                        player.PlayerTeamID = Convert.ToInt32(sqlDataReader["PlayerTeamID"]);
                        player.PlayerID = Convert.ToInt32(sqlDataReader["PlayerID"]);
                        player.PlayerNumber = Convert.ToInt32(sqlDataReader["PlayerNumber"]);
                        player.PlayerName = (string)sqlDataReader["PlayerName"];
                        player.PlayerIsActive = Convert.ToBoolean(sqlDataReader["PlayerIsActive"]);
                        try
                        {
                            player.PlayerImage = (byte[])sqlDataReader["PlayerImage"];
                        }
                        catch
                        {
                        }
                        try
                        {
                            player.PlayerIsReservePlayer = Convert.ToBoolean(sqlDataReader["PlayerIsReservePlayer"]);
                        }
                        catch
                        {
                        }
                        try
                        {
                            player.PlayerIsGoalkeeper = Convert.ToBoolean(sqlDataReader["PlayerIsGoalkeeper"]);
                        }
                        catch
                        {
                        }
                        try
                        {
                            player.PlayerIsCoach = Convert.ToBoolean(sqlDataReader["PlayerIsCoach"]);
                        }
                        catch
                        {
                        }
                        playerList.Add(player);
                    }
                    sqlDataReader.Close();
                }
                catch
                {
                    SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Player WHERE PlayerTeamID = " + TeamID.ToString() + " ORDER BY PlayerNumber", _conn);
                    sqlCommand.ExecuteReader().Close();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    while (sqlDataReader.Read())
                    {
                        Player player = new Player();
                        player.PlayerTeamID = Convert.ToInt32(sqlDataReader["PlayerTeamID"]);
                        player.PlayerID = Convert.ToInt32(sqlDataReader["PlayerID"]);
                        player.PlayerNumber = Convert.ToInt32(sqlDataReader["PlayerNumber"]);
                        player.PlayerName = (string)sqlDataReader["PlayerName"];
                        player.PlayerIsActive = Convert.ToBoolean(sqlDataReader["PlayerIsActive"]);
                        try
                        {
                            player.PlayerImage = (byte[])sqlDataReader["PlayerImage"];
                        }
                        catch
                        {
                        }
                        try
                        {
                            player.PlayerIsReservePlayer = Convert.ToBoolean(sqlDataReader["PlayerIsReservePlayer"]);
                        }
                        catch
                        {
                        }
                        try
                        {
                            player.PlayerIsGoalkeeper = Convert.ToBoolean(sqlDataReader["PlayerIsGoalkeeper"]);
                        }
                        catch
                        {
                        }
                        try
                        {
                            player.PlayerIsCoach = Convert.ToBoolean(sqlDataReader["PlayerIsCoach"]);
                        }
                        catch
                        {
                        }
                        playerList.Add(player);
                    }
                    sqlDataReader.Close();
                }
            }
            return playerList;
        }

        public Player[] ReadCoachesWithImage(int TeamID)
        {
            Player[] playerArray = new Player[0];
            DataTable dataTable = new DataTable("Coachess");
            if (_conn.State == ConnectionState.Open)
            {
                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Player WHERE PlayerTeamID = " + TeamID.ToString() + " AND PlayerNumber < 0 ORDER BY PlayerNumber", _conn);
                SqlDataReader sqlDataReader1 = sqlCommand.ExecuteReader();
                int length = 0;
                while (sqlDataReader1.Read())
                    ++length;
                sqlDataReader1.Close();
                SqlDataReader sqlDataReader2 = sqlCommand.ExecuteReader();
                playerArray = new Player[length];
                int index = 0;
                while (sqlDataReader2.Read())
                {
                    playerArray[index] = new Player();
                    playerArray[index].PlayerTeamID = Convert.ToInt32(sqlDataReader2["PlayerTeamID"]);
                    playerArray[index].PlayerID = Convert.ToInt32(sqlDataReader2["PlayerID"]);
                    playerArray[index].PlayerNumber = Convert.ToInt32(sqlDataReader2["PlayerNumber"]);
                    playerArray[index].PlayerName = (string)sqlDataReader2["PlayerName"];
                    playerArray[index].PlayerIsActive = true;
                    try
                    {
                        playerArray[index].PlayerImage = (byte[])sqlDataReader2["PlayerImage"];
                    }
                    catch
                    {
                    }
                    playerArray[index].PlayerIsReservePlayer = false;
                    ++index;
                }
                sqlDataReader2.Close();
            }
            return playerArray;
        }

        public List<GamePlayer> ReadGameCoachesWithImage(int TeamID)
        {
            List<GamePlayer> gamePlayerList = new List<GamePlayer>();
            DataTable dataTable = new DataTable("Coachess");
            if (_conn.State == ConnectionState.Open)
            {
                SqlDataReader sqlDataReader = new SqlCommand("SELECT * FROM Player WHERE PlayerTeamID = " + TeamID.ToString() + " AND PlayerNumber < 0 ORDER BY PlayerNumber DESC", _conn).ExecuteReader();
                while (sqlDataReader.Read())
                {
                    GamePlayer gamePlayer = new GamePlayer()
                    {
                        TeamID = Convert.ToInt32(sqlDataReader["PlayerTeamID"]),
                        PlayerNumber = Convert.ToInt32(sqlDataReader["PlayerNumber"])
                    };
                    gamePlayer.IsTrainer = gamePlayer.PlayerNumber == -1;
                    gamePlayer.IsCoTrainer = gamePlayer.PlayerNumber < -1;
                    gamePlayer.Name = (string)sqlDataReader["PlayerName"];
                    try
                    {
                        gamePlayer.PlayerImage = (byte[])sqlDataReader["PlayerImage"];
                    }
                    catch
                    {
                        gamePlayer.PlayerImage = (byte[])null;
                    }
                    gamePlayer.IsReservePlayer = false;
                    gamePlayerList.Add(gamePlayer);
                }
                sqlDataReader.Close();
            }
            return gamePlayerList;
        }

        public void DeletePlayer(Player PlayerToBeDeleted)
        {
            new SqlCommand("DELETE FROM Player WHERE PlayerID = " + PlayerToBeDeleted.PlayerID.ToString(), _conn).ExecuteNonQuery();
        }

        public void SavePlayers(List<Player> PlayersToBeSaved)
        {
            for (int index = 0; index < PlayersToBeSaved.Count; ++index)
                SavePlayer(PlayersToBeSaved[index]);
        }

        public int SavePlayer(Player PlayerToBeSaved)
        {
            if (PlayerToBeSaved.PlayerName == null || PlayerToBeSaved.PlayerName == string.Empty)
                PlayerToBeSaved.PlayerName = string.Empty + (object)'\n' + (object)'\r';
            if (PlayerToBeSaved.PlayerName == null)
                return -1;
            SqlCommand command1 = _conn.CreateCommand();
            SqlDataAdapter sqlDataAdapter1 = new SqlDataAdapter(command1);
            DataSet dataSet1 = new DataSet();
            command1.CommandType = CommandType.StoredProcedure;
            command1.CommandText = nameof(SavePlayer);
            command1.Parameters.Add("@PlayerID", SqlDbType.Int).Value = (object)PlayerToBeSaved.PlayerID;
            command1.Parameters.Add("@PlayerTeamID", SqlDbType.Int).Value = (object)PlayerToBeSaved.PlayerTeamID;
            command1.Parameters.Add("@PlayerNumber", SqlDbType.Int).Value = (object)PlayerToBeSaved.PlayerNumber;
            command1.Parameters.Add("@PlayerName", SqlDbType.NChar).Value = (object)PlayerToBeSaved.PlayerName.Trim();
            if (PlayerToBeSaved.PlayerIsActive)
                command1.Parameters.Add("@PlayerIsActive", SqlDbType.Bit).Value = (object)1;
            else
                command1.Parameters.Add("@PlayerIsActive", SqlDbType.Bit).Value = (object)0;
            if (PlayerToBeSaved.PlayerIsReservePlayer)
                command1.Parameters.Add("@PlayerIsReservePlayer", SqlDbType.Bit).Value = (object)1;
            else
                command1.Parameters.Add("@PlayerIsReservePlayer", SqlDbType.Bit).Value = (object)0;
            if (PlayerToBeSaved.PlayerIsGoalkeeper)
                command1.Parameters.Add("@PlayerIsGoalkeeper", SqlDbType.Bit).Value = (object)1;
            else
                command1.Parameters.Add("@PlayerIsGoalkeeper", SqlDbType.Bit).Value = (object)0;
            if (PlayerToBeSaved.PlayerIsCoach)
                command1.Parameters.Add("@PlayerIsCoach", SqlDbType.Bit).Value = (object)1;
            else
                command1.Parameters.Add("@PlayerIsCoach", SqlDbType.Bit).Value = (object)0;
            SqlParameter sqlParameter1 = new SqlParameter("@ID", SqlDbType.Int);
            sqlParameter1.Direction = ParameterDirection.Output;
            command1.Parameters.Add(sqlParameter1);
            try
            {
                sqlDataAdapter1.Fill(dataSet1, "ID");
                return Convert.ToInt32(sqlParameter1.Value);
            }
            catch
            {
                SqlCommand command2 = _conn.CreateCommand();
                SqlDataAdapter sqlDataAdapter2 = new SqlDataAdapter(command2);
                DataSet dataSet2 = new DataSet();
                command2.CommandType = CommandType.StoredProcedure;
                command2.CommandText = nameof(SavePlayer);
                command2.Parameters.Add("@PlayerID", SqlDbType.Int).Value = (object)PlayerToBeSaved.PlayerID;
                command2.Parameters.Add("@PlayerTeamID", SqlDbType.Int).Value = (object)PlayerToBeSaved.PlayerTeamID;
                command2.Parameters.Add("@PlayerNumber", SqlDbType.Int).Value = (object)PlayerToBeSaved.PlayerNumber;
                command2.Parameters.Add("@PlayerName", SqlDbType.NChar).Value = (object)PlayerToBeSaved.PlayerName.Trim();
                if (PlayerToBeSaved.PlayerIsActive)
                    command2.Parameters.Add("@PlayerIsActive", SqlDbType.Bit).Value = (object)1;
                else
                    command2.Parameters.Add("@PlayerIsActive", SqlDbType.Bit).Value = (object)0;
                if (PlayerToBeSaved.PlayerIsReservePlayer)
                    command2.Parameters.Add("@PlayerIsReservePlayer", SqlDbType.Bit).Value = (object)1;
                else
                    command2.Parameters.Add("@PlayerIsReservePlayer", SqlDbType.Bit).Value = (object)0;
                SqlParameter sqlParameter2 = new SqlParameter("@ID", SqlDbType.Int);
                sqlParameter2.Direction = ParameterDirection.Output;
                command2.Parameters.Add(sqlParameter2);
                try
                {
                    sqlDataAdapter2.Fill(dataSet2, "ID");
                    return Convert.ToInt32(sqlParameter2.Value);
                }
                catch
                {
                    SqlCommand command3 = _conn.CreateCommand();
                    SqlDataAdapter sqlDataAdapter3 = new SqlDataAdapter(command3);
                    DataSet dataSet3 = new DataSet();
                    command3.CommandType = CommandType.StoredProcedure;
                    command3.CommandText = nameof(SavePlayer);
                    command3.Parameters.Add("@PlayerID", SqlDbType.Int).Value = (object)PlayerToBeSaved.PlayerID;
                    command3.Parameters.Add("@PlayerTeamID", SqlDbType.Int).Value = (object)PlayerToBeSaved.PlayerTeamID;
                    command3.Parameters.Add("@PlayerNumber", SqlDbType.Int).Value = (object)PlayerToBeSaved.PlayerNumber;
                    command3.Parameters.Add("@PlayerName", SqlDbType.NChar).Value = (object)PlayerToBeSaved.PlayerName.Trim();
                    if (PlayerToBeSaved.PlayerIsActive)
                        command3.Parameters.Add("@PlayerIsActive", SqlDbType.Bit).Value = (object)1;
                    else
                        command3.Parameters.Add("@PlayerIsActive", SqlDbType.Bit).Value = (object)0;
                    SqlParameter sqlParameter3 = new SqlParameter("@ID", SqlDbType.Int);
                    sqlParameter3.Direction = ParameterDirection.Output;
                    command3.Parameters.Add(sqlParameter3);
                    try
                    {
                        sqlDataAdapter3.Fill(dataSet3, "ID");
                        return Convert.ToInt32(sqlParameter3.Value);
                    }
                    catch
                    {
                        return -1;
                    }
                }
            }
        }

        public int SavePlayerWithImage(Player PlayerToBeSaved)
        {
            if (PlayerToBeSaved.PlayerName == null)
                return -1;
            SqlCommand command = _conn.CreateCommand();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
            DataSet dataSet = new DataSet();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = nameof(SavePlayerWithImage);
            command.Parameters.Add("@PlayerID", SqlDbType.Int).Value = (object)PlayerToBeSaved.PlayerID;
            command.Parameters.Add("@PlayerTeamID", SqlDbType.Int).Value = (object)PlayerToBeSaved.PlayerTeamID;
            command.Parameters.Add("@PlayerNumber", SqlDbType.Int).Value = (object)PlayerToBeSaved.PlayerNumber;
            command.Parameters.Add("@PlayerName", SqlDbType.NChar).Value = (object)PlayerToBeSaved.PlayerName;
            command.Parameters.Add("@PlayerImage", SqlDbType.Image).Value = (object)PlayerToBeSaved.PlayerImage;
            if (PlayerToBeSaved.PlayerIsActive)
                command.Parameters.Add("@PlayerIsActive", SqlDbType.Bit).Value = (object)1;
            else
                command.Parameters.Add("@PlayerIsActive", SqlDbType.Bit).Value = (object)0;
            if (PlayerToBeSaved.PlayerIsReservePlayer)
                command.Parameters.Add("@PlayerIsReservePlayer", SqlDbType.Bit).Value = (object)1;
            else
                command.Parameters.Add("@PlayerIsReservePlayer", SqlDbType.Bit).Value = (object)0;
            if (PlayerToBeSaved.PlayerIsGoalkeeper)
                command.Parameters.Add("@PlayerIsGoalkeeper", SqlDbType.Bit).Value = (object)1;
            else
                command.Parameters.Add("@PlayerIsGoalkeeper", SqlDbType.Bit).Value = (object)0;
            if (PlayerToBeSaved.PlayerIsCoach)
                command.Parameters.Add("@PlayerIsCoach", SqlDbType.Bit).Value = (object)1;
            else
                command.Parameters.Add("@PlayerIsCoach", SqlDbType.Bit).Value = (object)0;
            SqlParameter sqlParameter = new SqlParameter("@ID", SqlDbType.Int);
            sqlParameter.Direction = ParameterDirection.Output;
            command.Parameters.Add(sqlParameter);
            try
            {
                sqlDataAdapter.Fill(dataSet, "ID");
            }
            catch
            {
            }
            return Convert.ToInt32(sqlParameter.Value);
        }

        public List<GamePlayer> GamePlayers(int GameID, int TeamID)
        {
            List<GamePlayer> gamePlayerList = new List<GamePlayer>();
            DataTable dataTable = new DataTable(nameof(GamePlayers));
            if (_conn.State == ConnectionState.Open)
            {
                try
                {
                    SqlDataReader sqlDataReader = new SqlCommand("SELECT  * FROM GamePlayerData WHERE GamePlayerGameID = " + GameID.ToString() + " AND GamePlayerTeamID = " + TeamID.ToString() + " ORDER BY GamePlayerIsReservePlayer, GamePlayerIsCoach DESC, GamePlayerIsGoalkeeper DESC, GamePlayerPlayerNumber", _conn).ExecuteReader();
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
                catch
                {
                    try
                    {
                        SqlDataReader sqlDataReader = new SqlCommand("SELECT  * FROM GamePlayerData WHERE GamePlayerGameID = " + GameID.ToString() + " AND GamePlayerTeamID = " + TeamID.ToString() + " ORDER BY GamePlayerIsReservePlayer, GamePlayerPlayerNumber", _conn).ExecuteReader();
                        dataTable.Columns.Add("GamePlayerPlayerNumber");
                        dataTable.Columns.Add("GamePlayerName");
                        dataTable.Columns.Add("GamePlayerPoints");
                        dataTable.Columns.Add("GamePlayerFouls");
                        dataTable.Columns.Add("GamePlayerIsReservePlayer");
                        dataTable.Load((IDataReader)sqlDataReader);
                        sqlDataReader.Close();
                    }
                    catch
                    {
                        SqlDataReader sqlDataReader = new SqlCommand("SELECT  * FROM GamePlayerData WHERE GamePlayerGameID = " + GameID.ToString() + " AND GamePlayerTeamID = " + TeamID.ToString() + " ORDER BY GamePlayerPlayerNumber", _conn).ExecuteReader();
                        dataTable.Columns.Add("GamePlayerPlayerNumber");
                        dataTable.Columns.Add("GamePlayerName");
                        dataTable.Columns.Add("GamePlayerPoints");
                        dataTable.Columns.Add("GamePlayerFouls");
                        dataTable.Load((IDataReader)sqlDataReader);
                        sqlDataReader.Close();
                    }
                }
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
                    try
                    {
                        gamePlayer.IsReservePlayer = Convert.ToBoolean(dataTable.Rows[index].ItemArray[4]);
                    }
                    catch
                    {
                    }
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
            if (_conn.State == ConnectionState.Open)
            {
                SqlDataReader sqlDataReader = new SqlCommand("SELECT  * FROM GamePlayerData WHERE GamePlayerGameID = " + GameID.ToString() + " AND GamePlayerTeamID = " + TeamID.ToString() + " ORDER BY GamePlayerGameID, GamePlayerIsCoach DESC, GamePlayerIsGoalkeeper DESC, GamePlayerPlayerNumber", _conn).ExecuteReader();
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

        public void SaveGamePlayers(List<GamePlayer> PlayersToBeSaved)
        {
            for (int index = 0; index < PlayersToBeSaved.Count; ++index)
                SaveGamePlayer(PlayersToBeSaved[index]);
        }

        public void SaveGamePlayers(GamePlayer[] PlayersToBeSaved)
        {
            for (int index = 0; index < PlayersToBeSaved.Length; ++index)
                SaveGamePlayer(PlayersToBeSaved[index]);
        }

        public void SaveGamePlayer(GamePlayer PlayerToBeSaved)
        {
            try
            {
                SqlCommand command = _conn.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = nameof(SaveGamePlayer);
                command.Parameters.Add("@GamePlayerGameID", SqlDbType.Int).Value = (object)PlayerToBeSaved.GameID;
                command.Parameters.Add("@GamePlayerTeamID", SqlDbType.Int).Value = (object)PlayerToBeSaved.TeamID;
                command.Parameters.Add("@GamePlayerPlayerNumber", SqlDbType.Int).Value = (object)PlayerToBeSaved.PlayerNumber;
                command.Parameters.Add("@GamePlayerName", SqlDbType.NChar).Value = (object)PlayerToBeSaved.Name;
                command.Parameters.Add("@GamePlayerPoints", SqlDbType.Int).Value = (object)PlayerToBeSaved.Points;
                command.Parameters.Add("@GamePlayerFouls", SqlDbType.Int).Value = (object)PlayerToBeSaved.Fouls;
                command.Parameters.Add("@GamePlayerCards", SqlDbType.Int).Value = (object)PlayerToBeSaved.GamePlayerCards;
                if (PlayerToBeSaved.IsReservePlayer)
                    command.Parameters.Add("@GamePlayerIsReservePlayer", SqlDbType.Bit).Value = (object)1;
                else
                    command.Parameters.Add("@gamePlayerIsReservePlayer", SqlDbType.Bit).Value = (object)0;
                if (PlayerToBeSaved.IsGoalkeeper)
                    command.Parameters.Add("@GamePlayerIsGoalkeeper", SqlDbType.Bit).Value = (object)1;
                else
                    command.Parameters.Add("@GamePlayerIsGoalkeeper", SqlDbType.Bit).Value = (object)0;
                if (PlayerToBeSaved.IsCoach)
                    command.Parameters.Add("@GamePlayerIsCoach", SqlDbType.Bit).Value = (object)1;
                else
                    command.Parameters.Add("@GamePlayerIsCoach", SqlDbType.Bit).Value = (object)0;
                command.ExecuteNonQuery();
            }
            catch
            {
                try
                {
                    SqlCommand command = _conn.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = nameof(SaveGamePlayer);
                    command.Parameters.Add("@GamePlayerGameID", SqlDbType.Int).Value = (object)PlayerToBeSaved.GameID;
                    command.Parameters.Add("@GamePlayerTeamID", SqlDbType.Int).Value = (object)PlayerToBeSaved.TeamID;
                    command.Parameters.Add("@GamePlayerPlayerNumber", SqlDbType.Int).Value = (object)PlayerToBeSaved.PlayerNumber;
                    command.Parameters.Add("@GamePlayerName", SqlDbType.NChar).Value = (object)PlayerToBeSaved.Name;
                    command.Parameters.Add("@GamePlayerPoints", SqlDbType.Int).Value = (object)PlayerToBeSaved.Points;
                    command.Parameters.Add("@GamePlayerFouls", SqlDbType.Int).Value = (object)PlayerToBeSaved.Fouls;
                    command.Parameters.Add("@GamePlayerCards", SqlDbType.Int).Value = (object)PlayerToBeSaved.GamePlayerCards;
                    if (PlayerToBeSaved.IsReservePlayer)
                        command.Parameters.Add("@GamePlayerIsReservePlayer", SqlDbType.Bit).Value = (object)1;
                    else
                        command.Parameters.Add("@GamePlayerIsReservePlayer", SqlDbType.Bit).Value = (object)0;
                    command.ExecuteNonQuery();
                }
                catch
                {
                    try
                    {
                        SqlCommand command = _conn.CreateCommand();
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = nameof(SaveGamePlayer);
                        command.Parameters.Add("@GamePlayerGameID", SqlDbType.Int).Value = (object)PlayerToBeSaved.GameID;
                        command.Parameters.Add("@GamePlayerTeamID", SqlDbType.Int).Value = (object)PlayerToBeSaved.TeamID;
                        command.Parameters.Add("@GamePlayerPlayerNumber", SqlDbType.Int).Value = (object)PlayerToBeSaved.PlayerNumber;
                        command.Parameters.Add("@GamePlayerName", SqlDbType.NChar).Value = (object)PlayerToBeSaved.Name;
                        command.Parameters.Add("@GamePlayerPoints", SqlDbType.Int).Value = (object)PlayerToBeSaved.Points;
                        if (PlayerToBeSaved.IsReservePlayer)
                            command.Parameters.Add("@GamePlayerIsReservePlayer", SqlDbType.Bit).Value = (object)1;
                        else
                            command.Parameters.Add("@GamePlayerIsReservePlayer", SqlDbType.Bit).Value = (object)0;
                        command.Parameters.Add("@GamePlayerFouls", SqlDbType.Int).Value = (object)PlayerToBeSaved.Fouls;
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        try
                        {
                            SqlCommand command = _conn.CreateCommand();
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
                        catch
                        {
                        }
                    }
                }
            }
        }

        public void DeleteGamePlayers(int GameID, int TeamID)
        {
            new SqlCommand("DELETE FROM GamePlayerData WHERE GamePlayerGameID = " + GameID.ToString() + " AND GamePlayerTeamID = " + TeamID.ToString(), _conn).ExecuteNonQuery();
        }

        public void DeleteGamePlayer(GamePlayer PlayerToBeDeleted)
        {
            new SqlCommand("DELETE FROM GamePlayerData WHERE GamePlayerGameID = " + PlayerToBeDeleted.GameID.ToString() + " AND GamePlayerTeamID = " + PlayerToBeDeleted.TeamID.ToString() + " AND GamePlayerPlayerNumber = " + PlayerToBeDeleted.PlayerNumber.ToString(), _conn).ExecuteNonQuery();
        }

        public void ShowGamePlayersWaterpolo(
          Form Target,
          List<GamePlayer> GamePlayerHome,
          List<GamePlayer> GamePlayerGuest,
          long MaxPlayerFouls)
        {
            Color lightGray = Color.LightGray;
            int count = GamePlayerHome.Count;
            if (GamePlayerGuest.Count > GamePlayerHome.Count)
                count = GamePlayerGuest.Count;
            for (int index1 = 0; index1 < count; ++index1)
            {
                Control.ControlCollection controls1 = Target.Controls;
                int num = index1 + 1;
                string key = "NoH" + num.ToString();
                if (controls1.ContainsKey(key))
                {
                    if (index1 < GamePlayerHome.Count)
                    {
                        GamePlayer gamePlayer = GamePlayerHome[index1];
                        Color color = MaxPlayerFouls >= 1L ? ((long)gamePlayer.Fouls < MaxPlayerFouls ? Color.LightGray : Color.Red) : (!gamePlayer.IsReservePlayer ? Color.LightGray : Color.Gray);
                        if (Target.Controls["NoH" + (index1 + 1).ToString()] != null)
                        {
                            Target.Controls["NoH" + (index1 + 1).ToString()].ForeColor = color;
                            Target.Controls["NoH" + (index1 + 1).ToString()].Text = gamePlayer.PlayerNumber.ToString();
                        }
                        if (Target.Controls["NameH" + (index1 + 1).ToString()] != null)
                        {
                            Target.Controls["NameH" + (index1 + 1).ToString()].ForeColor = color;
                            Target.Controls["NameH" + (index1 + 1).ToString()].Text = gamePlayer.Name.Trim();
                        }
                        if (Target.Controls["FoulsH" + (index1 + 1).ToString()] != null)
                        {
                            if ((long)gamePlayer.Fouls >= MaxPlayerFouls)
                                Target.Controls["FoulsH" + (index1 + 1).ToString()].ForeColor = color;
                            else
                                Target.Controls["FoulsH" + (index1 + 1).ToString()].ForeColor = Color.Lime;
                            switch (gamePlayer.Fouls)
                            {
                                case 0:
                                    Target.Controls["FoulsH" + (index1 + 1).ToString()].Text = " ";
                                    break;
                                case 1:
                                    Target.Controls["FoulsH" + (index1 + 1).ToString()].Text = "o";
                                    break;
                                case 2:
                                    Target.Controls["FoulsH" + (index1 + 1).ToString()].Text = "oo";
                                    break;
                                case 3:
                                    Target.Controls["FoulsH" + (index1 + 1).ToString()].Text = "ooo";
                                    break;
                                case 4:
                                    Target.Controls["FoulsH" + (index1 + 1).ToString()].Text = "oooo";
                                    break;
                                case 5:
                                    Target.Controls["FoulsH" + (index1 + 1).ToString()].Text = "ooooo";
                                    break;
                                case 6:
                                    Target.Controls["FoulsH" + (index1 + 1).ToString()].Text = "oooooo";
                                    break;
                                default:
                                    Target.Controls["FoulsH" + (index1 + 1).ToString()].Text = gamePlayer.Fouls.ToString();
                                    break;
                            }
                        }
                        if (Target.Controls["PointsH" + (index1 + 1).ToString()] != null)
                        {
                            Target.Controls["PointsH" + (index1 + 1).ToString()].ForeColor = color;
                            Target.Controls["PointsH" + (index1 + 1).ToString()].Text = gamePlayer.Points.ToString();
                        }
                        if (Target.Controls["CardsH" + (index1 + 1).ToString()] != null)
                        {
                            Target.Controls["CardsH" + (index1 + 1).ToString()].ForeColor = color;
                            Target.Controls["CardsH" + (index1 + 1).ToString()].Text = string.Empty;
                            if (gamePlayer.GamePlayerCards > 0)
                            {
                                Target.Controls["CardsH" + (index1 + 1).ToString()].Text = "█";
                                if (gamePlayer.GamePlayerCards == 1)
                                    Target.Controls["CardsH" + (index1 + 1).ToString()].ForeColor = Color.Yellow;
                                else
                                    Target.Controls["CardsH" + (index1 + 1).ToString()].ForeColor = Color.Red;
                            }
                        }
                    }
                    else
                    {
                        if (Target.Controls["NoH" + (index1 + 1).ToString()] != null)
                            Target.Controls["NoH" + (index1 + 1).ToString()].Text = string.Empty;
                        if (Target.Controls["NameH" + (index1 + 1).ToString()] != null)
                            Target.Controls["NameH" + (index1 + 1).ToString()].Text = string.Empty;
                        if (Target.Controls["FoulsH" + (index1 + 1).ToString()] != null)
                            Target.Controls["FoulsH" + (index1 + 1).ToString()].Text = string.Empty;
                        if (Target.Controls["PointsH" + (index1 + 1).ToString()] != null)
                            Target.Controls["PointsH" + (index1 + 1).ToString()].Text = string.Empty;
                        if (Target.Controls["CardsH" + (index1 + 1).ToString()] != null)
                            Target.Controls["CardsH" + (index1 + 1).ToString()].Text = string.Empty;
                    }
                    if (index1 < GamePlayerGuest.Count)
                    {
                        GamePlayer gamePlayer = GamePlayerGuest[index1];
                        Color color = MaxPlayerFouls >= 1L ? ((long)gamePlayer.Fouls < MaxPlayerFouls ? Color.LightGray : Color.Red) : (!gamePlayer.IsReservePlayer ? Color.LightGray : Color.Gray);
                        if (Target.Controls["NoG" + (index1 + 1).ToString()] != null)
                        {
                            Target.Controls["NoG" + (index1 + 1).ToString()].ForeColor = color;
                            Target.Controls["NoG" + (index1 + 1).ToString()].Text = gamePlayer.PlayerNumber.ToString();
                        }
                        if (Target.Controls["NameG" + (index1 + 1).ToString()] != null)
                        {
                            Target.Controls["NameG" + (index1 + 1).ToString()].ForeColor = color;
                            Target.Controls["NameG" + (index1 + 1).ToString()].Text = gamePlayer.Name.Trim();
                        }
                        if (Target.Controls["FoulsG" + (index1 + 1).ToString()] != null)
                        {
                            if ((long)gamePlayer.Fouls >= MaxPlayerFouls)
                                Target.Controls["FoulsG" + (index1 + 1).ToString()].ForeColor = color;
                            else
                                Target.Controls["FoulsG" + (index1 + 1).ToString()].ForeColor = Color.Lime;
                            switch (gamePlayer.Fouls)
                            {
                                case 0:
                                    Target.Controls["FoulsG" + (index1 + 1).ToString()].Text = " ";
                                    break;
                                case 1:
                                    Target.Controls["FoulsG" + (index1 + 1).ToString()].Text = "o";
                                    break;
                                case 2:
                                    Target.Controls["FoulsG" + (index1 + 1).ToString()].Text = "oo";
                                    break;
                                case 3:
                                    Target.Controls["FoulsG" + (index1 + 1).ToString()].Text = "ooo";
                                    break;
                                case 4:
                                    Target.Controls["FoulsG" + (index1 + 1).ToString()].Text = "oooo";
                                    break;
                                case 5:
                                    Control.ControlCollection controls2 = Target.Controls;
                                    num = index1 + 1;
                                    string index2 = "FoulsG" + num.ToString();
                                    controls2[index2].Text = "ooooo";
                                    break;
                                case 6:
                                    Control.ControlCollection controls3 = Target.Controls;
                                    num = index1 + 1;
                                    string index3 = "FoulsG" + num.ToString();
                                    controls3[index3].Text = "oooooo";
                                    break;
                                default:
                                    Control.ControlCollection controls4 = Target.Controls;
                                    num = index1 + 1;
                                    string index4 = "FoulsG" + num.ToString();
                                    Control control = controls4[index4];
                                    num = gamePlayer.Fouls;
                                    string str = num.ToString();
                                    control.Text = str;
                                    break;
                            }
                        }
                        Control.ControlCollection controls5 = Target.Controls;
                        num = index1 + 1;
                        string index5 = "PointsG" + num.ToString();
                        if (controls5[index5] != null)
                        {
                            Control.ControlCollection controls2 = Target.Controls;
                            num = index1 + 1;
                            string index2 = "PointsG" + num.ToString();
                            controls2[index2].ForeColor = color;
                            Control.ControlCollection controls3 = Target.Controls;
                            num = index1 + 1;
                            string index3 = "PointsG" + num.ToString();
                            Control control = controls3[index3];
                            num = gamePlayer.Points;
                            string str = num.ToString();
                            control.Text = str;
                        }
                        Control.ControlCollection controls6 = Target.Controls;
                        num = index1 + 1;
                        string index6 = "CardsG" + num.ToString();
                        if (controls6[index6] != null)
                        {
                            Control.ControlCollection controls2 = Target.Controls;
                            num = index1 + 1;
                            string index2 = "CardsG" + num.ToString();
                            controls2[index2].ForeColor = color;
                            Control.ControlCollection controls3 = Target.Controls;
                            num = index1 + 1;
                            string index3 = "CardsG" + num.ToString();
                            controls3[index3].Text = string.Empty;
                            if (gamePlayer.GamePlayerCards > 0)
                            {
                                Control.ControlCollection controls4 = Target.Controls;
                                num = index1 + 1;
                                string index4 = "CardsG" + num.ToString();
                                controls4[index4].Text = "█";
                                if (gamePlayer.GamePlayerCards == 1)
                                {
                                    Control.ControlCollection controls7 = Target.Controls;
                                    num = index1 + 1;
                                    string index7 = "CardsG" + num.ToString();
                                    controls7[index7].ForeColor = Color.Yellow;
                                }
                                else
                                {
                                    Control.ControlCollection controls7 = Target.Controls;
                                    num = index1 + 1;
                                    string index7 = "CardsG" + num.ToString();
                                    controls7[index7].ForeColor = Color.Red;
                                }
                            }
                        }
                    }
                    else
                    {
                        Control.ControlCollection controls2 = Target.Controls;
                        num = index1 + 1;
                        string index2 = "NoG" + num.ToString();
                        if (controls2[index2] != null)
                        {
                            Control.ControlCollection controls3 = Target.Controls;
                            num = index1 + 1;
                            string index3 = "NoG" + num.ToString();
                            controls3[index3].Text = string.Empty;
                        }
                        Control.ControlCollection controls4 = Target.Controls;
                        num = index1 + 1;
                        string index4 = "NameG" + num.ToString();
                        if (controls4[index4] != null)
                        {
                            Control.ControlCollection controls3 = Target.Controls;
                            num = index1 + 1;
                            string index3 = "NameG" + num.ToString();
                            controls3[index3].Text = string.Empty;
                        }
                        Control.ControlCollection controls5 = Target.Controls;
                        num = index1 + 1;
                        string index5 = "FoulsG" + num.ToString();
                        if (controls5[index5] != null)
                        {
                            Control.ControlCollection controls3 = Target.Controls;
                            num = index1 + 1;
                            string index3 = "FoulsG" + num.ToString();
                            controls3[index3].Text = string.Empty;
                        }
                        Control.ControlCollection controls6 = Target.Controls;
                        num = index1 + 1;
                        string index6 = "PointsG" + num.ToString();
                        if (controls6[index6] != null)
                        {
                            Control.ControlCollection controls3 = Target.Controls;
                            num = index1 + 1;
                            string index3 = "PointsG" + num.ToString();
                            controls3[index3].Text = string.Empty;
                        }
                        Control.ControlCollection controls7 = Target.Controls;
                        num = index1 + 1;
                        string index7 = "CardsG" + num.ToString();
                        if (controls7[index7] != null)
                        {
                            Control.ControlCollection controls3 = Target.Controls;
                            num = index1 + 1;
                            string index3 = "CardsG" + num.ToString();
                            controls3[index3].Text = string.Empty;
                        }
                    }
                }
            }
        }

        public void ClearGamePlayersList(
          Form Target,
          List<GamePlayer> GamePlayerHome,
          List<GamePlayer> GamePlayerGuest,
          long NoOfRows)
        {
            Color lightGray = Color.LightGray;
            for (int index = 0; (long)index < NoOfRows; ++index)
            {
                if (Target.Controls.ContainsKey("NoH" + (index + 1).ToString()))
                {
                    if (Target.Controls["NoH" + (index + 1).ToString()] != null)
                        Target.Controls["NoH" + (index + 1).ToString()].Text = string.Empty;
                    if (Target.Controls["NameH" + (index + 1).ToString()] != null)
                        Target.Controls["NameH" + (index + 1).ToString()].Text = string.Empty;
                    if (Target.Controls["FoulsH" + (index + 1).ToString()] != null)
                        Target.Controls["FoulsH" + (index + 1).ToString()].Text = string.Empty;
                    if (Target.Controls["PointsH" + (index + 1).ToString()] != null)
                        Target.Controls["PointsH" + (index + 1).ToString()].Text = string.Empty;
                    if (Target.Controls["CardsH" + (index + 1).ToString()] != null)
                        Target.Controls["CardsH" + (index + 1).ToString()].Text = string.Empty;
                    if (Target.Controls["NoG" + (index + 1).ToString()] != null)
                        Target.Controls["NoG" + (index + 1).ToString()].Text = string.Empty;
                    if (Target.Controls["NameG" + (index + 1).ToString()] != null)
                        Target.Controls["NameG" + (index + 1).ToString()].Text = string.Empty;
                    if (Target.Controls["FoulsG" + (index + 1).ToString()] != null)
                        Target.Controls["FoulsG" + (index + 1).ToString()].Text = string.Empty;
                    if (Target.Controls["PointsG" + (index + 1).ToString()] != null)
                        Target.Controls["PointsG" + (index + 1).ToString()].Text = string.Empty;
                    if (Target.Controls["CardsG" + (index + 1).ToString()] != null)
                        Target.Controls["CardsG" + (index + 1).ToString()].Text = string.Empty;
                }
            }
        }

        public void ShowGamePlayers(
          Form Target,
          List<GamePlayer> GamePlayerHome,
          List<GamePlayer> GamePlayerGuest,
          long MaxPlayerFouls)
        {
            Color lightGray = Color.LightGray;
            int count = GamePlayerHome.Count;
            if (GamePlayerGuest.Count > GamePlayerHome.Count)
                count = GamePlayerGuest.Count;
            for (int index = 0; index < count; ++index)
            {
                if (Target.Controls.ContainsKey("NoH" + (index + 1).ToString()))
                {
                    if (index < GamePlayerHome.Count)
                    {
                        GamePlayer gamePlayer = GamePlayerHome[index];
                        Color color = MaxPlayerFouls >= 1L ? ((long)gamePlayer.Fouls < MaxPlayerFouls ? Color.LightGray : Color.Red) : (!gamePlayer.IsReservePlayer ? Color.LightGray : Color.Gray);
                        if (Target.Controls["NoH" + (index + 1).ToString()] != null)
                        {
                            Target.Controls["NoH" + (index + 1).ToString()].ForeColor = color;
                            Target.Controls["NoH" + (index + 1).ToString()].Text = gamePlayer.PlayerNumber.ToString();
                        }
                        if (Target.Controls["NameH" + (index + 1).ToString()] != null)
                        {
                            Target.Controls["NameH" + (index + 1).ToString()].ForeColor = color;
                            Target.Controls["NameH" + (index + 1).ToString()].Text = gamePlayer.Name.Trim();
                        }
                        if (Target.Controls["FoulsH" + (index + 1).ToString()] != null)
                        {
                            if ((long)gamePlayer.Fouls >= MaxPlayerFouls)
                                Target.Controls["FoulsH" + (index + 1).ToString()].ForeColor = color;
                            else
                                Target.Controls["FoulsH" + (index + 1).ToString()].ForeColor = Color.Lime;
                            if (gamePlayer.Fouls < 1)
                                Target.Controls["FoulsH" + (index + 1).ToString()].Text = "0";
                            else
                                Target.Controls["FoulsH" + (index + 1).ToString()].Text = gamePlayer.Fouls.ToString();
                        }
                        if (Target.Controls["PointsH" + (index + 1).ToString()] != null)
                        {
                            Target.Controls["PointsH" + (index + 1).ToString()].ForeColor = color;
                            if (gamePlayer.Points < 1)
                                Target.Controls["PointsH" + (index + 1).ToString()].Text = "0";
                            else
                                Target.Controls["PointsH" + (index + 1).ToString()].Text = gamePlayer.Points.ToString();
                        }
                        if (Target.Controls["CardsH" + (index + 1).ToString()] != null)
                        {
                            Target.Controls["CardsH" + (index + 1).ToString()].ForeColor = color;
                            Target.Controls["CardsH" + (index + 1).ToString()].Text = string.Empty;
                            if (gamePlayer.GamePlayerCards > 0)
                            {
                                Target.Controls["CardsH" + (index + 1).ToString()].Text = "█";
                                if (gamePlayer.GamePlayerCards == 1)
                                    Target.Controls["CardsH" + (index + 1).ToString()].ForeColor = Color.Yellow;
                                else
                                    Target.Controls["CardsH" + (index + 1).ToString()].ForeColor = Color.Red;
                            }
                        }
                    }
                    else
                    {
                        if (Target.Controls["NoH" + (index + 1).ToString()] != null)
                            Target.Controls["NoH" + (index + 1).ToString()].Text = string.Empty;
                        if (Target.Controls["NameH" + (index + 1).ToString()] != null)
                            Target.Controls["NameH" + (index + 1).ToString()].Text = string.Empty;
                        if (Target.Controls["FoulsH" + (index + 1).ToString()] != null)
                            Target.Controls["FoulsH" + (index + 1).ToString()].Text = string.Empty;
                        if (Target.Controls["PointsH" + (index + 1).ToString()] != null)
                            Target.Controls["PointsH" + (index + 1).ToString()].Text = string.Empty;
                        if (Target.Controls["CardsH" + (index + 1).ToString()] != null)
                            Target.Controls["CardsH" + (index + 1).ToString()].Text = string.Empty;
                    }
                    if (index < GamePlayerGuest.Count)
                    {
                        GamePlayer gamePlayer = GamePlayerGuest[index];
                        Color color = MaxPlayerFouls >= 1L ? ((long)gamePlayer.Fouls < MaxPlayerFouls ? Color.LightGray : Color.Red) : (!gamePlayer.IsReservePlayer ? Color.LightGray : Color.Gray);
                        if (Target.Controls["NoG" + (index + 1).ToString()] != null)
                        {
                            Target.Controls["NoG" + (index + 1).ToString()].ForeColor = color;
                            Target.Controls["NoG" + (index + 1).ToString()].Text = gamePlayer.PlayerNumber.ToString();
                        }
                        if (Target.Controls["NameG" + (index + 1).ToString()] != null)
                        {
                            Target.Controls["NameG" + (index + 1).ToString()].ForeColor = color;
                            Target.Controls["NameG" + (index + 1).ToString()].Text = gamePlayer.Name.Trim();
                        }
                        if (Target.Controls["FoulsG" + (index + 1).ToString()] != null)
                        {
                            if ((long)gamePlayer.Fouls >= MaxPlayerFouls)
                                Target.Controls["FoulsG" + (index + 1).ToString()].ForeColor = color;
                            else
                                Target.Controls["FoulsG" + (index + 1).ToString()].ForeColor = Color.Lime;
                            if (gamePlayer.Fouls < 1)
                                Target.Controls["FoulsG" + (index + 1).ToString()].Text = "0";
                            else
                                Target.Controls["FoulsG" + (index + 1).ToString()].Text = gamePlayer.Fouls.ToString();
                        }
                        if (Target.Controls["PointsG" + (index + 1).ToString()] != null)
                        {
                            Target.Controls["PointsG" + (index + 1).ToString()].ForeColor = color;
                            if (gamePlayer.Points < 1)
                                Target.Controls["PointsG" + (index + 1).ToString()].Text = "0";
                            else
                                Target.Controls["PointsG" + (index + 1).ToString()].Text = gamePlayer.Points.ToString();
                        }
                        if (Target.Controls["CardsG" + (index + 1).ToString()] != null)
                        {
                            Target.Controls["CardsG" + (index + 1).ToString()].ForeColor = color;
                            Target.Controls["CardsG" + (index + 1).ToString()].Text = string.Empty;
                            if (gamePlayer.GamePlayerCards > 0)
                            {
                                Target.Controls["CardsG" + (index + 1).ToString()].Text = "█";
                                if (gamePlayer.GamePlayerCards == 1)
                                    Target.Controls["CardsG" + (index + 1).ToString()].ForeColor = Color.Yellow;
                                else
                                    Target.Controls["CardsG" + (index + 1).ToString()].ForeColor = Color.Red;
                            }
                        }
                    }
                    else
                    {
                        if (Target.Controls["NoG" + (index + 1).ToString()] != null)
                            Target.Controls["NoG" + (index + 1).ToString()].Text = string.Empty;
                        if (Target.Controls["NameG" + (index + 1).ToString()] != null)
                            Target.Controls["NameG" + (index + 1).ToString()].Text = string.Empty;
                        if (Target.Controls["FoulsG" + (index + 1).ToString()] != null)
                            Target.Controls["FoulsG" + (index + 1).ToString()].Text = string.Empty;
                        if (Target.Controls["PointsG" + (index + 1).ToString()] != null)
                            Target.Controls["PointsG" + (index + 1).ToString()].Text = string.Empty;
                        if (Target.Controls["CardsG" + (index + 1).ToString()] != null)
                            Target.Controls["CardsG" + (index + 1).ToString()].Text = string.Empty;
                    }
                }
            }
        }

        public Bitmap GamePlayerImage(int TeamID, int PlayerNumber)
        {
            Bitmap bitmap = (Bitmap)null;
            if (_conn.State == ConnectionState.Open)
            {
                SqlDataReader sqlDataReader = new SqlCommand("SELECT PlayerImage FROM Player WHERE PlayerTeamID = " + TeamID.ToString() + " AND PlayerNumber = " + PlayerNumber.ToString(), _conn).ExecuteReader();
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

        public List<Game> ReadGames(int GameKindID, bool IncreasingDate)
        {
            List<Game> gameList = new List<Game>();
            DataTable dataTable = new DataTable("Games");
            if (_conn.State == ConnectionState.Open)
            {
                string cmdText = "SELECT * FROM Game WHERE GameKindID = " + GameKindID.ToString() + " ORDER BY DateTime";
                if (IncreasingDate)
                    cmdText += " DESC";
                SqlDataReader sqlDataReader = new SqlCommand(cmdText, _conn).ExecuteReader();
                while (sqlDataReader.Read())
                {
                    Game game = new Game();
                    game.GameKindID = GameKindID;
                    game.ID = Convert.ToInt32(sqlDataReader["ID"]);
                    game.DateTime = Convert.ToDateTime(sqlDataReader["DateTime"]);
                    game.Started = Convert.ToBoolean(sqlDataReader["Started"]);
                    game.Period = Convert.ToInt32(sqlDataReader["Period"]);
                    game.HomeTeamID = Convert.ToInt32(sqlDataReader["HomeTeamID"]);
                    game.HomeTeamName = sqlDataReader["HomeTeamName"].ToString().Trim();
                    try
                    {
                        game.HomeTeamLogo = ByteArrayToImage((byte[])sqlDataReader["HomeTeamLogo"]);
                    }
                    catch
                    {
                    }
                    game.GuestTeamID = Convert.ToInt32(sqlDataReader["GuestTeamID"]);
                    game.GuestTeamName = sqlDataReader["GuestTeamName"].ToString().Trim();
                    try
                    {
                        game.GuestTeamLogo = ByteArrayToImage((byte[])sqlDataReader["GuestTeamLogo"]);
                    }
                    catch
                    {
                    }
                    gameList.Add(game);
                }
                sqlDataReader.Close();
            }
            return gameList;
        }

        public void SaveGames(Game[] MyGames)
        {
            for (int index = 0; index < MyGames.Length; ++index)
                SaveGame(MyGames[index]);
        }

        public int SaveGame(Game MyGame)
        {
            int id = MyGame.ID;
            SqlCommand command = _conn.CreateCommand();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
            DataSet dataSet = new DataSet();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = nameof(SaveGame);
            command.Parameters.Add("@ID", SqlDbType.Int).Value = (object)MyGame.ID;
            command.Parameters.Add("@GameKindID", SqlDbType.Int).Value = (object)MyGame.GameKindID;
            command.Parameters.Add("@DateTime", SqlDbType.DateTime).Value = (object)MyGame.DateTime;
            command.Parameters.Add("@IsStarted", SqlDbType.Bit).Value = (object)MyGame.Started;
            command.Parameters.Add("@Period", SqlDbType.Int).Value = (object)MyGame.Period;
            command.Parameters.Add("@HomeTeamID", SqlDbType.Int).Value = (object)MyGame.HomeTeamID;
            command.Parameters.Add("@HomeTeamName", SqlDbType.NChar).Value = (object)MyGame.HomeTeamName;
            command.Parameters.Add("@HomeTeamLogo", SqlDbType.Image).Value = (object)ImageToByteArray(MyGame.HomeTeamLogo);
            command.Parameters.Add("@GuestTeamID", SqlDbType.Int).Value = (object)MyGame.GuestTeamID;
            command.Parameters.Add("@GuestTeamName", SqlDbType.NChar).Value = (object)MyGame.GuestTeamName;
            command.Parameters.Add("@GuestTeamLogo", SqlDbType.Image).Value = (object)ImageToByteArray(MyGame.GuestTeamLogo);
            SqlParameter sqlParameter = new SqlParameter("@RetID", SqlDbType.Int);
            sqlParameter.Direction = ParameterDirection.Output;
            command.Parameters.Add(sqlParameter);
            try
            {
                sqlDataAdapter.Fill(dataSet, "ID");
            }
            catch
            {
            }
            return Convert.ToInt32(sqlParameter.Value);
        }

        public void DeleteGame(Game MyGame)
        {
            new SqlCommand("DELETE FROM Game WHERE ID = " + MyGame.ID.ToString(), _conn).ExecuteNonQuery();
            new SqlCommand("DELETE FROM GameData WHERE GameID = " + MyGame.ID.ToString(), _conn).ExecuteNonQuery();
            new SqlCommand("DELETE FROM GamePlayerData WHERE GamePlayerGameID = " + MyGame.ID.ToString(), _conn).ExecuteNonQuery();
        }

        public Dictionary<string, GameData> GameData(int GameID)
        {
            Dictionary<string, GameData> dictionary = (Dictionary<string, GameData>)null;
            DataTable dataTable = new DataTable(nameof(GameData));
            if (_conn.State == ConnectionState.Open)
            {
                SqlDataReader sqlDataReader = new SqlCommand("SELECT * FROM Gamedata WHERE GameID = " + GameID.ToString(), _conn).ExecuteReader();
                dictionary = new Dictionary<string, GameData>();
                while (sqlDataReader.Read())
                {
                    try
                    {
                        dictionary.Add(sqlDataReader["FieldName"].ToString().Trim(), new GameData(GameID, sqlDataReader["FieldName"].ToString().Trim(), Convert.ToInt64(sqlDataReader["FieldValue"]), sqlDataReader["FieldText"].ToString()));
                    }
                    catch
                    {
                        dictionary.Add(sqlDataReader["FieldName"].ToString().Trim(), new GameData(GameID, sqlDataReader["FieldName"].ToString().Trim(), Convert.ToInt64(sqlDataReader["FieldValue"]), sqlDataReader["FieldValue"].ToString()));
                    }
                }
                sqlDataReader.Close();
            }
            if (dictionary.Count == 0)
                dictionary = (Dictionary<string, GameData>)null;
            return dictionary;
        }

        public void SaveGameData(Form Source, int GameID)
        {
            foreach (Control control in (ArrangedElementCollection)Source.Controls)
            {
                try
                {
                    if (control.GetType() == typeof(Label) && !control.Name.StartsWith("lbl") && !control.Name.StartsWith("label"))
                        SaveGameData((Label)control, GameID);
                    control.GetType();
                    if (control.GetType() == typeof(ListBox))
                    {
                        if (control.Name.StartsWith("lb"))
                            SaveGameTextData((ListBox)control, GameID);
                    }
                }
                catch
                {
                }
            }
        }

        public void SaveGameTextData(ListBox Source, int GameID)
        {
            SqlCommand sqlCommand = new SqlCommand("DELETE FROM GameData WHERE GameID = " + GameID.ToString() + " AND FieldName = '" + Source.Name + "'", _conn);
            sqlCommand.ExecuteNonQuery();
            sqlCommand.CommandText = "INSERT INTO GameData (GameID, FieldName, FieldText) VALUES (" + GameID.ToString() + ",'" + Source.Name + "','";
            for (int index = 0; index < Source.Items.Count; ++index)
            {
                if (index > 0)
                    sqlCommand.CommandText += "|";
                sqlCommand.CommandText += Source.Items[index].ToString();
            }
            sqlCommand.CommandText += "')";
            try
            {
                sqlCommand.ExecuteNonQuery();
            }
            catch
            {
            }
        }

        public void SaveGameData(Label Source, int GameID)
        {
            try
            {
                if (!Source.Name.StartsWith("TimeOuts") && !Source.Name.StartsWith("AllTimeOuts") && (!Source.Name.StartsWith("Period") && !Source.Name.StartsWith("Service")) && (!Source.Name.StartsWith("Changes") && !Source.Name.StartsWith("SetScore") && (!Source.Name.StartsWith("SetsWon") && !Source.Name.StartsWith("PenaltyNo"))) && (!Source.Name.StartsWith("SetsWon") && !Source.Name.StartsWith("SetH") && (!Source.Name.StartsWith("SetG") && !Source.Name.StartsWith("GameH")) && (!Source.Name.StartsWith("GameG") && !Source.Name.StartsWith("Score") && !Source.Name.StartsWith("Corners"))))
                    return;
                long num = 0;
                if (Source.Text == "A")
                    num = -1L;
                else if (Source.Text == "B")
                    num = -2L;
                else if (Source.Text == "C")
                    num = -3L;
                else if (Source.Text == "D")
                    num = -4L;
                else if (Source.Text == "M")
                    num = -5L;
                else if (Source.Text == string.Empty)
                {
                    num = 0L;
                }
                else
                {
                    try
                    {
                        if (!Source.Name.StartsWith("TimeOuts"))
                        {
                            if (!Source.Name.StartsWith("Service"))
                            {
                                if (!Source.Name.StartsWith("Changes"))
                                    num = Convert.ToInt64(Source.Text);
                            }
                        }
                    }
                    catch
                    {
                    }
                }
                if (Source.Name.StartsWith("Period") && Source.Text == "E")
                    num = 5L;
                if (Source.Name.StartsWith("TimeOuts") || Source.Name.StartsWith("Service") || Source.Name.StartsWith("Changes"))
                    SaveGameData(Source.Name, Convert.ToInt64(Source.Text.Trim().Length), GameID);
                else
                    SaveGameData(Source.Name, num, GameID);
            }
            catch
            {
            }
        }

        public void SaveGameData(string FieldName, long Value, int GameID)
        {
            SqlCommand selectCommand = new SqlCommand(nameof(SaveGameData), _conn);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand);
            DataSet dataSet = new DataSet();
            selectCommand.CommandType = CommandType.StoredProcedure;
            selectCommand.Parameters.Add(new SqlParameter("@GameID", (object)GameID));
            selectCommand.Parameters.Add(new SqlParameter("@Name", (object)FieldName));
            selectCommand.Parameters.Add(new SqlParameter("@Value", (object)Value));
            try
            {
                sqlDataAdapter.Fill(dataSet, nameof(GameID));
            }
            catch
            {
            }
        }

        public void DeleteGameDataset(string FieldName, int GameID)
        {
            SqlCommand selectCommand = new SqlCommand(nameof(DeleteGameDataset), _conn);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand);
            DataSet dataSet = new DataSet();
            selectCommand.CommandType = CommandType.StoredProcedure;
            selectCommand.Parameters.Add(new SqlParameter("@GameID", (object)GameID));
            selectCommand.Parameters.Add(new SqlParameter("@Name", (object)FieldName));
            try
            {
                sqlDataAdapter.Fill(dataSet, nameof(GameID));
            }
            catch
            {
            }
        }

        public void DeleteGameDatasets(string FieldName, int GameID)
        {
            new SqlCommand("DELETE FROM GameData WHERE GameID = " + GameID.ToString() + " AND FieldName LIKE '" + FieldName + "%'", _conn).ExecuteNonQuery();
        }

        public Dictionary<string, GameData> LoadGameData(Form Target, int GameID)
        {
            Dictionary<string, GameData> dictionary = GameData(GameID);
            if (dictionary != null)
            {
                foreach (Control control in (ArrangedElementCollection)Target.Controls)
                {
                    if (control.GetType() == typeof(Label) && dictionary.ContainsKey(control.Name) && (control.Name.StartsWith("TimeOuts") || control.Name.StartsWith("AllTimeOuts") || (control.Name.StartsWith("Period") || control.Name.StartsWith("Service")) || (control.Name.StartsWith("PlayerChanges") || control.Name.StartsWith("Score"))))
                        control.Text = control.Name.StartsWith("TimeOuts") || control.Name.StartsWith("Service") || control.Name.StartsWith("PlayerChanges") ? string.Empty.PadLeft(Convert.ToInt32(dictionary[control.Name].Value), 'o') : (!control.Name.StartsWith("Period") || dictionary[control.Name].Value <= 4L || !(Target.Text == "Basketball") ? dictionary[control.Name].Value.ToString() : "E");
                    if (control.GetType() == typeof(ListBox) && dictionary.ContainsKey(control.Name))
                    {
                        ((ListBox)control).Items.Clear();
                        string[] strArray = dictionary[control.Name].Text.Split('|');
                        for (int index = 0; index < strArray.Length; ++index)
                        {
                            if (strArray[index].Trim() != string.Empty)
                                ((ListBox)control).Items.Add((object)strArray[index]);
                        }
                      ((ListControl)control).SelectedIndex = ((ListBox)control).Items.Count - 1;
                    }
                }
            }
            return dictionary;
        }

        public long LoadTeamFouls(int GameID, bool FromHomeTeam)
        {
            Dictionary<string, GameData> dictionary = GameData(GameID);
            if (dictionary == null)
                return 0;
            if (FromHomeTeam)
            {
                if (dictionary.ContainsKey("TeamFoulsHome") && dictionary["TeamFoulsHome"] != null)
                    return dictionary["TeamFoulsHome"].Value;
                return 0;
            }
            if (dictionary.ContainsKey("TeamFoulsGuest") && dictionary["TeamFoulsGuest"] != null)
                return dictionary["TeamFoulsGuest"].Value;
            return 0;
        }

        public int GetMaxTennisSet(int GameID)
        {
            int num = -1;
            if (_conn.State == ConnectionState.Open)
            {
                SqlDataReader sqlDataReader = new SqlCommand("SELECT * FROM Gamedata WHERE GameID = " + GameID.ToString() + " AND FieldName = 'TennisMaxSets'", _conn).ExecuteReader();
                while (sqlDataReader.Read())
                    num = Convert.ToInt32(sqlDataReader["FieldValue"]);
                sqlDataReader.Close();
            }
            return num;
        }

        public long GetGameTimeMillisecond(int GameID)
        {
            long num = 0;
            if (_conn.State == ConnectionState.Open)
            {
                SqlDataReader sqlDataReader = new SqlCommand("SELECT * FROM Gamedata WHERE GameID = " + GameID.ToString() + " AND FieldName = 'GameTime'", _conn).ExecuteReader();
                while (sqlDataReader.Read())
                    num = (long)Convert.ToInt32(sqlDataReader["FieldValue"]);
                sqlDataReader.Close();
            }
            return num;
        }

        public byte[] ImageToByteArray(Image Picture)
        {
            if (Picture == null)
                return new byte[0];
            Bitmap bitmap = new Bitmap(Picture);
            MemoryStream memoryStream = new MemoryStream();
            bitmap.Save((Stream)memoryStream, ImageFormat.Jpeg);
            return memoryStream.ToArray();
        }

        public Image ByteArrayToImage(byte[] _array)
        {
            try
            {
                return (Image)new Bitmap((Stream)new MemoryStream(_array, 0, _array.Length));
            }
            catch
            {
                return (Image)null;
            }
        }

        public delegate void PercentCompleteEventHandler(DataBaseFunctions sender, int percent);
    }
}