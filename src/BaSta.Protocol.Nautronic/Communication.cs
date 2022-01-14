namespace BaSta.Protocol.Nautronic;

public class Communication
{
    public NauBeeChannels E_CHANNELS = NauBeeChannels.CHANNEL_0;
    public NaubeePowerLevel E_POWER_LEVELS = NaubeePowerLevel.POWERLEVEL_0;
    private NauBeeChannels CommunicationUpdateChannel_oldChannel = NauBeeChannels.NOF_COMMUNICATION_CHANNELS;
    public static uint[] communicationChannelIndex = { 13U, 17U, 22U, 26U, 15U, 19U, 11U, 24U };
    public static uint[] communicationPowerIndex = { 10U, 14U, 18U };
    private readonly cBoard _board;
    private const int CommunicationSetDBM_oldDBM = -1;

    public Communication(cBoard instance2) => _board = instance2;

    public void CommunicationUpdateChannel(NauCom Instance)
    {
        if (Instance.WirelessChannel < NauBeeChannels.CHANNEL_0 || Instance.WirelessChannel == CommunicationUpdateChannel_oldChannel)
            return;

        _board.Board_NauBee_SetChannel(communicationChannelIndex[(int) Instance.WirelessChannel] - 11U);
    }

    public void CommunicationSetDBM(NauCom Instance)
    {
        if (Instance.wirelessDBM < 0 || Instance.wirelessDBM == CommunicationSetDBM_oldDBM)
            return;
        _board.Board_NauBee_SetDBM(Communication.communicationPowerIndex[Instance.wirelessDBM]);
    }
}