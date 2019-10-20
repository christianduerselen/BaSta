using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BaSta.Game
{
    public class PlayerEditForm : Form
    {
        private NumericUpDown nudPlayerNumber;
        private TextBox txtPlayerName;
        private NumericUpDown nudPlayerPoints;
        private NumericUpDown nudPlayerFouls;
        private Label label1;
        private Label lblFouls;
        private Button btnDeletePlayer;
        private Label labelMsgBoxTitle;
        private Label labelMsgBoxText1;
        private Label labelMsgBoxText2;
        private CheckBox cbIsReserverPlayer;
        private RadioButton rbCardsNone;
        private RadioButton rbYellowCard;
        private RadioButton rbRedCard;
        private GamePlayer _player;

        private void InitializeComponent()
        {
            ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(PlayerEditForm));
            nudPlayerNumber = new NumericUpDown();
            txtPlayerName = new TextBox();
            nudPlayerPoints = new NumericUpDown();
            nudPlayerFouls = new NumericUpDown();
            label1 = new Label();
            lblFouls = new Label();
            btnDeletePlayer = new Button();
            labelMsgBoxTitle = new Label();
            labelMsgBoxText1 = new Label();
            labelMsgBoxText2 = new Label();
            cbIsReserverPlayer = new CheckBox();
            rbCardsNone = new RadioButton();
            rbYellowCard = new RadioButton();
            rbRedCard = new RadioButton();
            nudPlayerNumber.BeginInit();
            nudPlayerPoints.BeginInit();
            nudPlayerFouls.BeginInit();
            SuspendLayout();
            nudPlayerNumber.AccessibleDescription = (string)null;
            nudPlayerNumber.AccessibleName = (string)null;
            componentResourceManager.ApplyResources((object)nudPlayerNumber, "nudPlayerNumber");
            nudPlayerNumber.Font = (Font)null;
            nudPlayerNumber.Maximum = new Decimal(new int[4]
            {
        99,
        0,
        0,
        0
            });
            nudPlayerNumber.Minimum = new Decimal(new int[4]
            {
        2,
        0,
        0,
        int.MinValue
            });
            nudPlayerNumber.Name = "nudPlayerNumber";
            txtPlayerName.AccessibleDescription = (string)null;
            txtPlayerName.AccessibleName = (string)null;
            componentResourceManager.ApplyResources((object)txtPlayerName, "txtPlayerName");
            txtPlayerName.BackgroundImage = (Image)null;
            txtPlayerName.Font = (Font)null;
            txtPlayerName.Name = "txtPlayerName";
            nudPlayerPoints.AccessibleDescription = (string)null;
            nudPlayerPoints.AccessibleName = (string)null;
            componentResourceManager.ApplyResources((object)nudPlayerPoints, "nudPlayerPoints");
            nudPlayerPoints.Font = (Font)null;
            nudPlayerPoints.Name = "nudPlayerPoints";
            nudPlayerFouls.AccessibleDescription = (string)null;
            nudPlayerFouls.AccessibleName = (string)null;
            componentResourceManager.ApplyResources((object)nudPlayerFouls, "nudPlayerFouls");
            nudPlayerFouls.Font = (Font)null;
            nudPlayerFouls.Name = "nudPlayerFouls";
            label1.AccessibleDescription = (string)null;
            label1.AccessibleName = (string)null;
            componentResourceManager.ApplyResources((object)label1, "label1");
            label1.Font = (Font)null;
            label1.Name = "label1";
            lblFouls.AccessibleDescription = (string)null;
            lblFouls.AccessibleName = (string)null;
            componentResourceManager.ApplyResources((object)lblFouls, "lblFouls");
            lblFouls.Font = (Font)null;
            lblFouls.Name = "lblFouls";
            btnDeletePlayer.AccessibleDescription = (string)null;
            btnDeletePlayer.AccessibleName = (string)null;
            componentResourceManager.ApplyResources((object)btnDeletePlayer, "btnDeletePlayer");
            btnDeletePlayer.BackgroundImage = (Image)null;
            btnDeletePlayer.Font = (Font)null;
            btnDeletePlayer.Name = "btnDeletePlayer";
            btnDeletePlayer.UseVisualStyleBackColor = true;
            btnDeletePlayer.Click += new EventHandler(btnDeletePlayer_Click);
            labelMsgBoxTitle.AccessibleDescription = (string)null;
            labelMsgBoxTitle.AccessibleName = (string)null;
            componentResourceManager.ApplyResources((object)labelMsgBoxTitle, "labelMsgBoxTitle");
            labelMsgBoxTitle.Font = (Font)null;
            labelMsgBoxTitle.Name = "labelMsgBoxTitle";
            labelMsgBoxText1.AccessibleDescription = (string)null;
            labelMsgBoxText1.AccessibleName = (string)null;
            componentResourceManager.ApplyResources((object)labelMsgBoxText1, "labelMsgBoxText1");
            labelMsgBoxText1.Font = (Font)null;
            labelMsgBoxText1.Name = "labelMsgBoxText1";
            labelMsgBoxText2.AccessibleDescription = (string)null;
            labelMsgBoxText2.AccessibleName = (string)null;
            componentResourceManager.ApplyResources((object)labelMsgBoxText2, "labelMsgBoxText2");
            labelMsgBoxText2.Font = (Font)null;
            labelMsgBoxText2.Name = "labelMsgBoxText2";
            cbIsReserverPlayer.AccessibleDescription = (string)null;
            cbIsReserverPlayer.AccessibleName = (string)null;
            componentResourceManager.ApplyResources((object)cbIsReserverPlayer, "cbIsReserverPlayer");
            cbIsReserverPlayer.BackgroundImage = (Image)null;
            cbIsReserverPlayer.Font = (Font)null;
            cbIsReserverPlayer.Name = "cbIsReserverPlayer";
            cbIsReserverPlayer.UseVisualStyleBackColor = true;
            rbCardsNone.AccessibleDescription = (string)null;
            rbCardsNone.AccessibleName = (string)null;
            componentResourceManager.ApplyResources((object)rbCardsNone, "rbCardsNone");
            rbCardsNone.BackgroundImage = (Image)null;
            rbCardsNone.Checked = true;
            rbCardsNone.Font = (Font)null;
            rbCardsNone.Name = "rbCardsNone";
            rbCardsNone.TabStop = true;
            rbCardsNone.UseVisualStyleBackColor = true;
            rbYellowCard.AccessibleDescription = (string)null;
            rbYellowCard.AccessibleName = (string)null;
            componentResourceManager.ApplyResources((object)rbYellowCard, "rbYellowCard");
            rbYellowCard.BackgroundImage = (Image)null;
            rbYellowCard.Font = (Font)null;
            rbYellowCard.Name = "rbYellowCard";
            rbYellowCard.UseVisualStyleBackColor = true;
            rbRedCard.AccessibleDescription = (string)null;
            rbRedCard.AccessibleName = (string)null;
            componentResourceManager.ApplyResources((object)rbRedCard, "rbRedCard");
            rbRedCard.BackgroundImage = (Image)null;
            rbRedCard.Font = (Font)null;
            rbRedCard.Name = "rbRedCard";
            rbRedCard.UseVisualStyleBackColor = true;
            AccessibleDescription = (string)null;
            AccessibleName = (string)null;
            componentResourceManager.ApplyResources((object)this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)null;
            Controls.Add((Control)rbRedCard);
            Controls.Add((Control)rbYellowCard);
            Controls.Add((Control)rbCardsNone);
            Controls.Add((Control)cbIsReserverPlayer);
            Controls.Add((Control)labelMsgBoxText2);
            Controls.Add((Control)labelMsgBoxText1);
            Controls.Add((Control)labelMsgBoxTitle);
            Controls.Add((Control)btnDeletePlayer);
            Controls.Add((Control)lblFouls);
            Controls.Add((Control)nudPlayerFouls);
            Controls.Add((Control)nudPlayerPoints);
            Controls.Add((Control)txtPlayerName);
            Controls.Add((Control)nudPlayerNumber);
            Controls.Add((Control)label1);
            Font = (Font)null;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)null;
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = nameof(PlayerEditForm);
            FormClosing += new FormClosingEventHandler(PlayerEditForm_FormClosing);
            KeyDown += new KeyEventHandler(PlayerEditForm_KeyDown);
            nudPlayerNumber.EndInit();
            nudPlayerPoints.EndInit();
            nudPlayerFouls.EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        public GamePlayer Player
        {
            get
            {
                return _player;
            }
            set
            {
                _player = value;
            }
        }

        public PlayerEditForm(GamePlayer MyPlayer, int MaxFouls)
        {
            InitializeComponent();
            _player = MyPlayer;
            if (_player.PlayerNumber < 0)
            {
                nudPlayerNumber.Visible = false;
                cbIsReserverPlayer.Visible = false;
                nudPlayerPoints.Visible = false;
                label1.Visible = false;
            }
            nudPlayerNumber.Value = (Decimal)_player.PlayerNumber;
            txtPlayerName.Text = _player.Name.Trim();
            nudPlayerPoints.Value = (Decimal)_player.Points;
            cbIsReserverPlayer.Checked = MyPlayer.IsReservePlayer;
            rbYellowCard.Checked = _player.GamePlayerCards == 1;
            rbRedCard.Checked = _player.GamePlayerCards > 1;
            if (MaxFouls <= 0)
                return;
            lblFouls.Visible = true;
            nudPlayerFouls.Visible = true;
            nudPlayerFouls.Maximum = (Decimal)MaxFouls;
            nudPlayerFouls.Value = (Decimal)_player.Fouls;
        }

        private void PlayerEditForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (txtPlayerName.Text.Trim() != string.Empty)
            {
                _player.PlayerNumber = (int)nudPlayerNumber.Value;
                _player.Name = txtPlayerName.Text.Trim();
                _player.Points = (int)nudPlayerPoints.Value;
                _player.Fouls = (int)nudPlayerFouls.Value;
                _player.IsReservePlayer = cbIsReserverPlayer.Checked;
                if (rbCardsNone.Checked)
                    _player.GamePlayerCards = 0;
                if (rbYellowCard.Checked)
                    _player.GamePlayerCards = 1;
                if (!rbRedCard.Checked)
                    return;
                _player.GamePlayerCards = 2;
            }
            else
                _player = (GamePlayer)null;
        }

        private void btnDeletePlayer_Click(object sender, EventArgs e)
        {
            if (!(txtPlayerName.Text.Trim() != string.Empty) || MessageBox.Show(labelMsgBoxText1.Text + txtPlayerName.Text.Trim() + labelMsgBoxText2.Text, labelMsgBoxTitle.Text, MessageBoxButtons.OKCancel) != DialogResult.OK)
                return;
            txtPlayerName.Text = string.Empty;
            Close();
        }

        private void PlayerEditForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return)
                return;
            Close();
        }
    }
}