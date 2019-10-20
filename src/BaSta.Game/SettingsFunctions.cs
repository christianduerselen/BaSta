using System;
using System.Collections;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace BaSta.Game
{
    public class SettingsFunctions
    {
        public void saveParameters(Form Source, DataBaseFunctions DbFunc, int GameKindID, int Period)
        {
            GameParameter Parameter = (GameParameter)null;
            foreach (Control control in (ArrangedElementCollection)Source.Controls)
            {
                if (control.GetType() != typeof(Label) && control.GetType() != typeof(Button) && control.Name != nameof(Period))
                {
                    if (control.GetType() == typeof(NumericUpDown))
                    {
                        if (control.Tag != null)
                        {
                            try
                            {
                                long int64 = Convert.ToInt64(control.Tag);
                                Parameter = new GameParameter(GameKindID, Period, control.Name, int64);
                            }
                            catch
                            {
                            }
                        }
                    }
                    if (control.GetType() == typeof(CheckBox))
                        Parameter = !((CheckBox)control).Checked ? new GameParameter(GameKindID, Period, control.Name, 0L) : new GameParameter(GameKindID, Period, control.Name, 1L);
                    if (control.GetType() == typeof(RadioButton))
                        Parameter = !((RadioButton)control).Checked ? new GameParameter(GameKindID, Period, control.Name, 0L) : new GameParameter(GameKindID, Period, control.Name, 1L);
                    if (control.GetType() == typeof(ComboBox))
                    {
                        long selectedIndex = (long)((ListControl)control).SelectedIndex;
                        Parameter = new GameParameter(GameKindID, Period, control.Name, selectedIndex);
                    }
                    if (Parameter != null)
                        DbFunc.WriteGameParameter(Parameter);
                }
            }
        }

        public void showParameters(Form Target, DataBaseFunctions DbFunc, int GameKindID, int Period)
        {
            GameParameter[] gameParameterArray = DbFunc.GameParameter(GameKindID, Period);
            if (gameParameterArray == null)
                return;
            Hashtable hashtable = new Hashtable();
            for (int index = 0; index < gameParameterArray.Length; ++index)
                hashtable.Add((object)gameParameterArray[index].ParameterName.Trim(), (object)gameParameterArray[index]);
            foreach (Control control in (ArrangedElementCollection)Target.Controls)
            {
                if (hashtable.Contains((object)control.Name))
                {
                    if (control.GetType() == typeof(ComboBox))
                        ((ListControl)control).SelectedIndex = Convert.ToInt32(((GameParameter)hashtable[(object)control.Name]).ParameterIntValue);
                    if (control.GetType() == typeof(CheckBox))
                        ((CheckBox)control).Checked = ((GameParameter)hashtable[(object)control.Name]).ParameterIntValue > 0L;
                    if (control.GetType() == typeof(RadioButton))
                        ((RadioButton)control).Checked = ((GameParameter)hashtable[(object)control.Name]).ParameterIntValue > 0L;
                    if (control.GetType() == typeof(NumericUpDown))
                    {
                        if (((NumericUpDown)control).Minimum < new Decimal(1))
                            ((NumericUpDown)control).Value = new Decimal(0);
                        else
                            ((NumericUpDown)control).Value = new Decimal(1);
                        Decimal num = Convert.ToDecimal(((GameParameter)hashtable[(object)control.Name]).ParameterIntValue);
                        string upper = control.Name.ToUpper();
                        if (upper.Contains("TIME") && !upper.Contains("TIMEOUTS") && (!upper.Contains("SCORE") && !upper.Contains("BREAKTIME")))
                        {
                            num /= new Decimal(1000);
                            if ((!upper.Contains("PENALTY") || GameKindID != 4) && num > new Decimal(90))
                                num /= new Decimal(60);
                        }
                        else if (upper.Contains("BREAKTIME"))
                        {
                            if (GameKindID == 1)
                            {
                                num /= new Decimal(1000);
                            }
                            else
                            {
                                num /= new Decimal(1000);
                                if (num > new Decimal(59))
                                    num /= new Decimal(60);
                            }
                        }
                      ((NumericUpDown)control).Value = num;
                    }
                }
            }
        }
    }
}