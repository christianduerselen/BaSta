using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;

namespace BaSta.Scoreboard
{
  public class StramatelWaterSender : IScoreBoard
  {
    private static readonly byte[] _Seq01 = new byte[54]
    {
      (byte) 248,
      (byte) 156,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 48,
      (byte) 49,
      (byte) 48,
      (byte) 32,
      (byte) 32,
      (byte) 49,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 48,
      (byte) 48,
      (byte) 48,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 49,
      (byte) 32,
      (byte) 32,
      (byte) 13
    };
    private static readonly byte[] _Seq02 = new byte[54]
    {
      (byte) 248,
      (byte) 52,
      (byte) 32,
      (byte) 32,
      (byte) 53,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 13
    };
    private static readonly byte[] _Seq03 = new byte[54]
    {
      (byte) 248,
      (byte) 52,
      (byte) 32,
      (byte) 32,
      (byte) 53,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 13
    };
    private static readonly byte[] _Seq04 = new byte[54]
    {
      (byte) 248,
      (byte) 52,
      (byte) 32,
      (byte) 32,
      (byte) 53,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 13
    };
    private static readonly byte[] _Seq05 = new byte[1]
    {
      (byte) 1
    };
    private static readonly byte[] _Seq06 = new byte[54]
    {
      (byte) 248,
      (byte) 52,
      (byte) 32,
      (byte) 32,
      (byte) 53,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 13
    };
    private static readonly byte[] _Seq07 = new byte[54]
    {
      (byte) 248,
      (byte) 52,
      (byte) 32,
      (byte) 32,
      (byte) 53,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 13
    };
    private static readonly byte[] _Seq08 = new byte[54]
    {
      (byte) 248,
      (byte) 52,
      (byte) 32,
      (byte) 32,
      (byte) 53,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 13
    };
    private static readonly byte[] _Seq09 = new byte[1]
    {
      (byte) 2
    };
    private static readonly byte[] _Seq10 = new byte[54]
    {
      (byte) 248,
      (byte) 52,
      (byte) 32,
      (byte) 32,
      (byte) 53,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 13
    };
    private static readonly byte[] _Seq11 = new byte[54]
    {
      (byte) 248,
      (byte) 52,
      (byte) 32,
      (byte) 32,
      (byte) 53,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 13
    };
    private static readonly byte[] _Seq12 = new byte[54]
    {
      (byte) 248,
      (byte) 52,
      (byte) 32,
      (byte) 32,
      (byte) 53,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 13
    };
    private static readonly byte[] _Seq13 = new byte[1]
    {
      (byte) 3
    };
    private static readonly byte[] _Seq14 = new byte[54]
    {
      (byte) 248,
      (byte) 52,
      (byte) 32,
      (byte) 32,
      (byte) 53,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 13
    };
    private static readonly byte[] _Seq15 = new byte[54]
    {
      (byte) 248,
      (byte) 52,
      (byte) 32,
      (byte) 32,
      (byte) 53,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 13
    };
    private static readonly byte[] _Seq16 = new byte[54]
    {
      (byte) 248,
      (byte) 52,
      (byte) 32,
      (byte) 32,
      (byte) 53,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 13
    };
    private static readonly byte[] _Seq17 = new byte[1]
    {
      (byte) 4
    };
    private static readonly byte[] _Seq18 = new byte[54]
    {
      (byte) 248,
      (byte) 52,
      (byte) 32,
      (byte) 32,
      (byte) 53,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 13
    };
    private static readonly byte[] _Seq19 = new byte[54]
    {
      (byte) 248,
      (byte) 52,
      (byte) 32,
      (byte) 32,
      (byte) 53,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 13
    };
    private static readonly byte[] _Seq20 = new byte[54]
    {
      (byte) 248,
      (byte) 52,
      (byte) 32,
      (byte) 32,
      (byte) 53,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 13
    };
    private static readonly byte[] _Seq21 = new byte[1]
    {
      (byte) 5
    };
    private static readonly byte[] _Seq22 = new byte[54]
    {
      (byte) 248,
      (byte) 67,
      (byte) 52,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 49,
      (byte) 49,
      (byte) 49,
      (byte) 49,
      (byte) 32,
      (byte) 32,
      (byte) 49,
      (byte) 49,
      (byte) 49,
      (byte) 49,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 49,
      (byte) 49,
      (byte) 49,
      (byte) 49,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 13
    };
    private static readonly byte[] _Intermediate0 = new byte[3]
    {
      (byte) 224,
      (byte) 232,
      (byte) 228
    };
    private static readonly byte[] _TemplateX4D = new byte[54]
    {
      (byte) 248,
      (byte) 77,
      (byte) 58,
      (byte) 48,
      (byte) 48,
      (byte) 48,
      (byte) 48,
      (byte) 48,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 48,
      (byte) 49,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 48,
      (byte) 32,
      (byte) 13
    };
    private static readonly byte[] _Intermediate2 = new byte[3]
    {
      (byte) 224,
      (byte) 232,
      (byte) 228
    };
    private static readonly byte[] _TemplateX56 = new byte[54]
    {
      (byte) 248,
      (byte) 86,
      (byte) 48,
      (byte) 48,
      (byte) 48,
      (byte) 48,
      (byte) 48,
      (byte) 48,
      (byte) 48,
      (byte) 48,
      (byte) 32,
      (byte) 32,
      (byte) 48,
      (byte) 48,
      (byte) 48,
      (byte) 32,
      (byte) 32,
      (byte) 48,
      (byte) 48,
      (byte) 48,
      (byte) 49,
      (byte) 48,
      (byte) 48,
      (byte) 48,
      (byte) 48,
      (byte) 48,
      (byte) 48,
      (byte) 48,
      (byte) 48,
      (byte) 48,
      (byte) 48,
      (byte) 48,
      (byte) 48,
      (byte) 48,
      (byte) 48,
      (byte) 48,
      (byte) 48,
      (byte) 48,
      (byte) 48,
      (byte) 48,
      (byte) 48,
      (byte) 48,
      (byte) 48,
      (byte) 48,
      (byte) 48,
      (byte) 48,
      (byte) 48,
      (byte) 48,
      (byte) 32,
      (byte) 32,
      (byte) 32,
      (byte) 48,
      (byte) 32,
      (byte) 13
    };
    private int _version = 1;
    private byte[] _send_bytes = new byte[54];
    private byte[] _TempByte = new byte[(int) byte.MaxValue];
    private Sportart _sport = Sportart.Waterpolo;
    private int _refresh_interval = 100;
    private bool _init_scb = true;
    private short _serial_port = 1;
    private int _baudrate = 19200;
    private byte _brightness = byte.MaxValue;
    private byte _brightness_shotclock = byte.MaxValue;
    private byte _hornlevel = byte.MaxValue;
    private byte _hornlevel_shotclock = byte.MaxValue;
    private Dictionary<string, string> _fields = new Dictionary<string, string>();
    private const byte _start_byte = 248;
    private const byte _end_byte = 13;
    private int _StateMachineState;
    private int Loop;
    private int _TempSize;
    private Thread _output_thread;
    private System.IO.Ports.SerialPort _ser_out;
    private bool _refresh_home_team;
    private bool _refresh_guest_team;
    private bool _shotclock_horn;
    private bool _horn;
    private bool _time_running;
    private bool _redlight;
    private bool _sending_active;

    public bool RefreshTeams
    {
      set
      {
        if (!value)
          return;
        _refresh_home_team = true;
        _refresh_guest_team = true;
      }
    }

    public Dictionary<string, string> Fields
    {
      get
      {
        return _fields;
      }
      set
      {
        _fields = value;
      }
    }

    public Sportart Sport
    {
      get
      {
        return _sport;
      }
      set
      {
        _sport = value;
        if (_sport != Sportart.Handball && _sport != Sportart.Handball)
          return;
        _refresh_interval = 100;
      }
    }

    public int RefreshInterval
    {
      get
      {
        return _refresh_interval;
      }
      set
      {
        _refresh_interval = value;
      }
    }

    public bool DoRefreshHomeTeam
    {
      get
      {
        return _refresh_home_team;
      }
      set
      {
        _refresh_home_team = value;
      }
    }

    public bool DoRefreshGuestTeam
    {
      get
      {
        return _refresh_guest_team;
      }
      set
      {
        _refresh_guest_team = value;
      }
    }

    public short SerialPort
    {
      get
      {
        return _serial_port;
      }
      set
      {
        _serial_port = value;
        if (_ser_out.IsOpen)
          _ser_out.Close();
        _ser_out.PortName = "COM" + _serial_port.ToString();
      }
    }

    public int Baudrate
    {
      get
      {
        return _baudrate;
      }
      set
      {
        _baudrate = value;
        if (_ser_out.IsOpen)
          _ser_out.Close();
        _ser_out.BaudRate = _baudrate;
      }
    }

    public byte Brightness
    {
      get
      {
        return _brightness;
      }
      set
      {
        _brightness = value;
      }
    }

    public byte BrightnessShotclock
    {
      get
      {
        return _brightness_shotclock;
      }
      set
      {
        _brightness_shotclock = value;
      }
    }

    public byte Hornlevel
    {
      get
      {
        return _hornlevel;
      }
      set
      {
        _hornlevel = value;
      }
    }

    public byte HornlevelShotclock
    {
      get
      {
        return _hornlevel_shotclock;
      }
      set
      {
        _hornlevel_shotclock = value;
      }
    }

    public bool TimeRunning
    {
      get
      {
        return _time_running;
      }
      set
      {
        _time_running = value;
      }
    }

    public bool Horn
    {
      get
      {
        return _horn;
      }
      set
      {
        _horn = value;
        if (!_ser_out.IsOpen)
          return;
        _ser_out.DtrEnable = _horn;
      }
    }

    public bool ShotclockHorn
    {
      get
      {
        return _shotclock_horn;
      }
      set
      {
        _shotclock_horn = value;
        if (!_ser_out.IsOpen)
          return;
        _ser_out.RtsEnable = _shotclock_horn;
      }
    }

    public bool RedLight
    {
      get
      {
        return _redlight;
      }
      set
      {
        _redlight = value;
      }
    }

    public void Dispose()
    {
      if (_output_thread != null)
      {
        if (_output_thread.IsAlive)
          _output_thread.Abort();
      }
      try
      {
        if (_ser_out == null)
          return;
        _ser_out.Dispose();
      }
      catch
      {
      }
    }

    public void Clear()
    {
    }

    public void Refresh()
    {
    }

    public StramatelWaterSender()
    {
    }

    public StramatelWaterSender(int SerialPort, int RefreshInterval, int Version)
    {
      _version = Version;
      _refresh_interval = RefreshInterval;
      for (int index = 0; index < _send_bytes.Length; ++index)
        _send_bytes[index] = (byte) 0;
      _send_bytes[0] = (byte) 248;
      _send_bytes[2] = (byte) 32;
      _send_bytes[3] = (byte) 32;
      _send_bytes[53] = (byte) 13;
      _serial_port = Convert.ToInt16(SerialPort);
      _ser_out = new System.IO.Ports.SerialPort("COM" + _serial_port.ToString(), 19200, Parity.None, 8, StopBits.One);
      _ser_out.DtrEnable = false;
      _ser_out.RtsEnable = true;
      try
      {
        _ser_out.Open();
        _ser_out.DtrEnable = _horn;
        _ser_out.RtsEnable = _shotclock_horn;
      }
      catch
      {
      }
      Loop = 1;
      while (Loop < 22)
      {
        _TempSize = 0;
        switch (Loop)
        {
          case 1:
            _TempSize = StramatelWaterSender._Seq01.Length;
            Array.Copy((Array) StramatelWaterSender._Seq01, 0, (Array) _TempByte, 0, _TempSize);
            break;
          case 2:
            _TempSize = StramatelWaterSender._Seq02.Length;
            Array.Copy((Array) StramatelWaterSender._Seq02, 0, (Array) _TempByte, 0, _TempSize);
            break;
          case 3:
            _TempSize = StramatelWaterSender._Seq03.Length;
            Array.Copy((Array) StramatelWaterSender._Seq03, 0, (Array) _TempByte, 0, _TempSize);
            break;
          case 4:
            _TempSize = StramatelWaterSender._Seq04.Length;
            Array.Copy((Array) StramatelWaterSender._Seq04, 0, (Array) _TempByte, 0, _TempSize);
            break;
          case 5:
            _TempSize = StramatelWaterSender._Seq05.Length;
            Array.Copy((Array) StramatelWaterSender._Seq05, 0, (Array) _TempByte, 0, _TempSize);
            break;
          case 6:
            _TempSize = StramatelWaterSender._Seq06.Length;
            Array.Copy((Array) StramatelWaterSender._Seq06, 0, (Array) _TempByte, 0, _TempSize);
            break;
          case 7:
            _TempSize = StramatelWaterSender._Seq07.Length;
            Array.Copy((Array) StramatelWaterSender._Seq07, 0, (Array) _TempByte, 0, _TempSize);
            break;
          case 8:
            _TempSize = StramatelWaterSender._Seq08.Length;
            Array.Copy((Array) StramatelWaterSender._Seq08, 0, (Array) _TempByte, 0, _TempSize);
            break;
          case 9:
            _TempSize = StramatelWaterSender._Seq09.Length;
            Array.Copy((Array) StramatelWaterSender._Seq09, 0, (Array) _TempByte, 0, _TempSize);
            break;
          case 10:
            _TempSize = StramatelWaterSender._Seq10.Length;
            Array.Copy((Array) StramatelWaterSender._Seq10, 0, (Array) _TempByte, 0, _TempSize);
            break;
          case 11:
            _TempSize = StramatelWaterSender._Seq11.Length;
            Array.Copy((Array) StramatelWaterSender._Seq11, 0, (Array) _TempByte, 0, _TempSize);
            break;
          case 12:
            _TempSize = StramatelWaterSender._Seq12.Length;
            Array.Copy((Array) StramatelWaterSender._Seq12, 0, (Array) _TempByte, 0, _TempSize);
            break;
          case 13:
            _TempSize = StramatelWaterSender._Seq13.Length;
            Array.Copy((Array) StramatelWaterSender._Seq13, 0, (Array) _TempByte, 0, _TempSize);
            break;
          case 14:
            _TempSize = StramatelWaterSender._Seq14.Length;
            Array.Copy((Array) StramatelWaterSender._Seq14, 0, (Array) _TempByte, 0, _TempSize);
            break;
          case 15:
            _TempSize = StramatelWaterSender._Seq15.Length;
            Array.Copy((Array) StramatelWaterSender._Seq15, 0, (Array) _TempByte, 0, _TempSize);
            break;
          case 16:
            _TempSize = StramatelWaterSender._Seq16.Length;
            Array.Copy((Array) StramatelWaterSender._Seq16, 0, (Array) _TempByte, 0, _TempSize);
            break;
          case 17:
            _TempSize = StramatelWaterSender._Seq17.Length;
            Array.Copy((Array) StramatelWaterSender._Seq17, 0, (Array) _TempByte, 0, _TempSize);
            break;
          case 18:
            _TempSize = StramatelWaterSender._Seq18.Length;
            Array.Copy((Array) StramatelWaterSender._Seq18, 0, (Array) _TempByte, 0, _TempSize);
            break;
          case 19:
            _TempSize = StramatelWaterSender._Seq19.Length;
            Array.Copy((Array) StramatelWaterSender._Seq19, 0, (Array) _TempByte, 0, _TempSize);
            break;
          case 20:
            _TempSize = StramatelWaterSender._Seq20.Length;
            Array.Copy((Array) StramatelWaterSender._Seq20, 0, (Array) _TempByte, 0, _TempSize);
            break;
          case 21:
            _TempSize = StramatelWaterSender._Seq21.Length;
            Array.Copy((Array) StramatelWaterSender._Seq21, 0, (Array) _TempByte, 0, _TempSize);
            break;
          case 22:
            _TempSize = StramatelWaterSender._Seq22.Length;
            Array.Copy((Array) StramatelWaterSender._Seq22, 0, (Array) _TempByte, 0, _TempSize);
            break;
          default:
            _TempSize = 0;
            break;
        }
        ++Loop;
        if (_TempSize > 0)
        {
          try
          {
            _ser_out.Write(_TempByte, 0, _TempSize);
          }
          catch
          {
          }
          if (_TempSize > 2)
            Thread.Sleep(50);
          else
            Thread.Sleep(150);
        }
      }
      Thread.Sleep(250);
      _output_thread = new Thread(new ThreadStart(_output));
      _output_thread.Start();
    }

    public bool StartSending
    {
      set
      {
        _sending_active = value;
      }
    }

    public void SetFieldValue(string FieldName, string Value)
    {
      if (FieldName.StartsWith("Fouls") && Value.Trim() == string.Empty)
        return;
      if (_fields.ContainsKey(FieldName))
        _fields[FieldName] = Value;
      else
        _fields.Add(FieldName, Value);
    }

    private void _output()
    {
      if (!_ser_out.IsOpen)
      {
        try
        {
          _ser_out.Open();
          _ser_out.DtrEnable = _horn;
          _ser_out.RtsEnable = _shotclock_horn;
        }
        catch
        {
        }
      }
      int num = 0;
      while (true)
      {
        if (_sending_active)
        {
          try
          {
            _init_scb = false;
            if (_init_scb)
            {
              _init_scb = false;
              _make_telegram_erase_player_name();
              try
              {
                _ser_out.Write(_send_bytes, 0, _send_bytes.Length);
              }
              catch
              {
              }
              Thread.Sleep(250);
            }
            else
            {
              int length = _send_bytes.Length;
              int count = 0;
              _refresh_home_team = false;
              _refresh_guest_team = false;
              if (_refresh_home_team)
              {
                _refresh_home_team = false;
                Thread.Sleep(250);
                _make_telegram_team_name();
                try
                {
                  _ser_out.Write(_send_bytes, 0, _send_bytes.Length);
                }
                catch
                {
                }
                for (int index = 0; index < 14; ++index)
                {
                  Thread.Sleep(250);
                  _make_telegram_player_name(HomeGuestTeam.Home, index + 1);
                  try
                  {
                    _ser_out.Write(_send_bytes, 0, _send_bytes.Length);
                  }
                  catch
                  {
                  }
                }
              }
              else if (_refresh_guest_team)
              {
                _refresh_guest_team = false;
                Thread.Sleep(250);
                _make_telegram_team_name();
                try
                {
                  _ser_out.Write(_send_bytes, 0, _send_bytes.Length);
                }
                catch
                {
                }
                for (int index = 0; index < 14; ++index)
                {
                  Thread.Sleep(250);
                  _make_telegram_player_name(HomeGuestTeam.Guest, index + 1);
                  try
                  {
                    _ser_out.Write(_send_bytes, 0, _send_bytes.Length);
                  }
                  catch
                  {
                  }
                }
              }
              else
              {
                if (_sport == Sportart.Volleyball)
                {
                  switch (_StateMachineState)
                  {
                    case 0:
                      count = _make_telegram_intermediate();
                      ++_StateMachineState;
                      break;
                    case 1:
                      count = _make_telegram_volley();
                      ++_StateMachineState;
                      break;
                    case 2:
                      count = _make_telegram_intermediate();
                      ++_StateMachineState;
                      break;
                    case 3:
                      count = _make_telegram_volley();
                      _StateMachineState = 0;
                      break;
                    default:
                      _StateMachineState = 0;
                      break;
                  }
                }
                else if (_sport == Sportart.Icehockey || _sport == Sportart.Handball || (_sport == Sportart.Soccer || _sport == Sportart.Icehockey) || _sport == Sportart.Indoorsoccer)
                {
                  switch (_StateMachineState)
                  {
                    case 0:
                      count = _make_telegram_intermediate();
                      ++_StateMachineState;
                      break;
                    case 1:
                      count = _make_telegram_hand_soccer();
                      ++_StateMachineState;
                      break;
                    case 2:
                      count = _make_telegram_intermediate();
                      ++_StateMachineState;
                      break;
                    case 3:
                      count = _make_telegram_hand_soccer();
                      _StateMachineState = 0;
                      break;
                    default:
                      _StateMachineState = 0;
                      break;
                  }
                }
                else
                {
                  switch (num)
                  {
                    case 0:
                      if (_sport == Sportart.Basketball)
                        _make_telegram_basket_with_pers_fouls();
                      if (_sport == Sportart.Handball)
                        _make_telegram_hand_soccer();
                      if (_sport == Sportart.Soccer)
                        _make_telegram_hand_soccer();
                      if (_sport == Sportart.Icehockey)
                        _make_telegram_hand_soccer();
                      if (_sport == Sportart.Waterpolo)
                        _make_telegram_simple_timer();
                      if (_sport == Sportart.Indoorsoccer)
                      {
                        _make_telegram_hand_soccer();
                        break;
                      }
                      break;
                    case 1:
                      if (_sport == Sportart.Basketball)
                        _make_telegram_basket_hand_hockey_foot_with_points(MatchData.Mannschaft.Heim);
                      if (_sport == Sportart.Handball)
                        _make_telegram_basket_hand_hockey_foot_with_points(MatchData.Mannschaft.Heim);
                      if (_sport == Sportart.Icehockey)
                      {
                        _make_telegram_basket_hand_hockey_foot_with_points(MatchData.Mannschaft.Heim);
                        break;
                      }
                      break;
                    case 2:
                      if (_sport == Sportart.Basketball)
                        _make_telegram_basket_with_pers_fouls();
                      if (_sport == Sportart.Handball)
                        _make_telegram_hand_soccer();
                      if (_sport == Sportart.Soccer)
                        _make_telegram_hand_soccer();
                      if (_sport == Sportart.Icehockey)
                        _make_telegram_hand_soccer();
                      if (_sport == Sportart.Waterpolo)
                        _make_telegram_simple_timer();
                      if (_sport == Sportart.Indoorsoccer)
                      {
                        _make_telegram_hand_soccer();
                        break;
                      }
                      break;
                    case 3:
                      if (_sport == Sportart.Basketball)
                        _make_telegram_basket_hand_hockey_foot_with_points(MatchData.Mannschaft.Gast);
                      if (_sport == Sportart.Handball)
                        _make_telegram_basket_hand_hockey_foot_with_points(MatchData.Mannschaft.Gast);
                      if (_sport == Sportart.Icehockey)
                      {
                        _make_telegram_basket_hand_hockey_foot_with_points(MatchData.Mannschaft.Gast);
                        break;
                      }
                      break;
                    case 4:
                      if (_sport == Sportart.Basketball)
                        _make_telegram_basket_with_pers_fouls();
                      if (_sport == Sportart.Handball)
                        _make_telegram_hand_soccer();
                      if (_sport == Sportart.Soccer)
                        _make_telegram_hand_soccer();
                      if (_sport == Sportart.Icehockey)
                        _make_telegram_hand_soccer();
                      if (_sport == Sportart.Waterpolo)
                        _make_telegram_simple_timer();
                      if (_sport == Sportart.Indoorsoccer)
                      {
                        _make_telegram_hand_soccer();
                        break;
                      }
                      break;
                    case 5:
                      if (_sport == Sportart.Basketball)
                        _make_telegram_basket_with_pers_fouls();
                      if (_sport == Sportart.Handball)
                        _make_telegram_hand_soccer();
                      if (_sport == Sportart.Soccer)
                        _make_telegram_hand_soccer();
                      if (_sport == Sportart.Icehockey)
                        _make_telegram_hand_soccer();
                      if (_sport == Sportart.Waterpolo)
                        _make_telegram_simple_timer();
                      if (_sport == Sportart.Indoorsoccer)
                      {
                        _make_telegram_hand_soccer();
                        break;
                      }
                      break;
                    default:
                      if (_sport == Sportart.Basketball)
                      {
                        _make_telegram_basket_with_pers_fouls();
                        break;
                      }
                      break;
                  }
                  ++num;
                  if (num > 5)
                    num = 0;
                }
                try
                {
                  _ser_out.Write(_send_bytes, 0, count);
                }
                catch
                {
                }
              }
            }
          }
          catch
          {
            try
            {
              _ser_out.Close();
            }
            catch
            {
            }
          }
        }
        Thread.Sleep(_refresh_interval);
      }
    }

    private string GetFieldValue(string FieldName)
    {
      if (_fields.ContainsKey(FieldName))
        return _fields[FieldName].ToString();
      return string.Empty;
    }

    private void _insert_gametime()
    {
      _redlight = false;
      string str = GetFieldValue("GameTime").Trim();
      for (int index = 1; index < 53; ++index)
        _send_bytes[index] = (byte) 32;
      if (!str.Contains(".") && !str.Contains(":"))
        str = "0:" + str.Trim().PadLeft(2, '0');
      if (str.Contains(":"))
        str = str.PadLeft(5);
      else if (str.Contains("."))
        str = str.Trim() == "0.0" || _redlight ? " 0.00" : str.Trim().PadLeft(4, '0').PadRight(5);
      _send_bytes[4] = Convert.ToByte(str[0]);
      _send_bytes[5] = Convert.ToByte(str[1]);
      _send_bytes[6] = Convert.ToByte(str[3]);
      _send_bytes[7] = Convert.ToByte(str[4]);
    }

    private void _make_telegram_basket_with_pers_fouls()
    {
      _insert_gametime();
      _send_bytes[1] = (byte) 51;
      _send_bytes[3] = (byte) 48;
      string str1 = GetFieldValue("ScoreHome").PadLeft(3);
      _send_bytes[8] = (byte) str1[0];
      _send_bytes[9] = (byte) str1[1];
      _send_bytes[10] = (byte) str1[2];
      string str2 = GetFieldValue("ScoreGuest").PadLeft(3);
      _send_bytes[11] = (byte) str2[0];
      _send_bytes[12] = (byte) str2[1];
      _send_bytes[13] = (byte) str2[2];
      _send_bytes[14] = (byte) GetFieldValue("Period").PadLeft(1)[0];
      _send_bytes[15] = (byte) GetFieldValue("TeamFoulsHome").PadLeft(1)[0];
      _send_bytes[16] = (byte) GetFieldValue("TeamFoulsGuest").PadLeft(1)[0];
      _send_bytes[17] = (byte) (48 + GetFieldValue("TimeOutsHome").Trim().Length);
      _send_bytes[18] = (byte) (48 + GetFieldValue("TimeOutsGuest").Trim().Length);
      _send_bytes[19] = Form1.UseBasketHorn >= 1 ? (_horn ? (byte) 49 : (byte) 48) : (byte) 48;
      _send_bytes[20] = (byte) GetFieldValue("TimeRunning")[0];
      if (_redlight)
        _send_bytes[20] = (byte) 82;
      string str3 = GetFieldValue("TimeoutTimeHome").PadLeft(3);
      if (str3.Trim() == string.Empty)
        str3 = GetFieldValue("TimeoutTimeGuest").PadLeft(3);
      _send_bytes[21] = (byte) str3[0];
      string empty = string.Empty;
      for (int index = 0; index < 14; ++index)
      {
        string str4 = GetFieldValue("FoulsH" + (index + 1).ToString()).PadLeft(1);
        _send_bytes[22 + index] = (byte) str4[0];
      }
      for (int index = 0; index < 14; ++index)
      {
        string str4 = GetFieldValue("FoulsG" + (index + 1).ToString()).PadLeft(1);
        _send_bytes[34 + index] = (byte) str4[0];
      }
      _send_bytes[46] = (byte) str3[1];
      _send_bytes[47] = (byte) str3[2];
      string str5 = GetFieldValue("ShotTime").PadLeft(2);
      _send_bytes[48] = (byte) str5[0];
      _send_bytes[49] = (byte) str5[1];
      _send_bytes[50] = Form1.UseBasketHorn >= 1 ? (_shotclock_horn ? (byte) 49 : (byte) 48) : (byte) 48;
      byte num = (byte) GetFieldValue("SC_EdgeLightOn")[0];
      _send_bytes[51] = _shotclock_horn ? num : (byte) 48;
      _send_bytes[52] = (byte) 49;
    }

    private void _make_telegram_basket_hand_hockey_foot_with_points(MatchData.Mannschaft Team)
    {
      _insert_gametime();
      string str1 = "PointsH";
      _send_bytes[1] = (byte) 56;
      if (Team == MatchData.Mannschaft.Gast)
      {
        str1 = "PointsG";
        _send_bytes[1] = (byte) 55;
      }
      string empty = string.Empty;
      for (int index = 0; index < 14; ++index)
      {
        string str2 = GetFieldValue(str1 + (index + 1).ToString()).PadLeft(2);
        if (index > 12)
        {
          _send_bytes[11] = (byte) str2[0];
          _send_bytes[12] = (byte) str2[1];
        }
        else
        {
          _send_bytes[22 + index * 2] = (byte) str2[0];
          _send_bytes[23 + index * 2] = (byte) str2[1];
        }
      }
      _send_bytes[19] = Form1.UseBasketHorn >= 1 ? (_horn ? (byte) 49 : (byte) 48) : (byte) 48;
      _send_bytes[20] = !(GetFieldValue("TimeRunning") != string.Empty) ? (byte) 48 : (byte) GetFieldValue("TimeRunning")[0];
      if (_redlight)
        _send_bytes[20] = (byte) 82;
      string str3 = GetFieldValue("ShotTime").PadLeft(2);
      _send_bytes[48] = (byte) str3[0];
      _send_bytes[49] = (byte) str3[1];
      _send_bytes[50] = Form1.UseBasketHorn >= 1 ? (_shotclock_horn ? (byte) 49 : (byte) 48) : (byte) 48;
      byte num = !(GetFieldValue("SC_EdgeLightOn") != string.Empty) ? (byte) 48 : (byte) GetFieldValue("SC_EdgeLightOn")[0];
      _send_bytes[51] = _shotclock_horn ? num : (byte) 48;
      _send_bytes[52] = (byte) 49;
    }

    private int _make_telegram_hand_soccer()
    {
      int length = _send_bytes.Length;
      switch (_StateMachineState)
      {
        case 1:
          length = StramatelWaterSender._TemplateX4D.Length;
          Array.Copy((Array) StramatelWaterSender._TemplateX4D, 0, (Array) _send_bytes, 0, length);
          string str1 = GetFieldValue("GameTime").Trim();
          if (!str1.Contains(".") && !str1.Contains(":"))
            str1 = "0:" + str1.Trim().PadLeft(2, '0');
          if (str1.Contains(":"))
            str1 = str1.PadLeft(5);
          else if (str1.Contains("."))
            str1 = str1.Trim() == "0.0" || _redlight ? " 0.00" : str1.Trim().PadLeft(4, '0').PadRight(5);
          _send_bytes[4] = Convert.ToByte(str1[0]);
          _send_bytes[5] = Convert.ToByte(str1[1]);
          _send_bytes[6] = Convert.ToByte(str1[3]);
          _send_bytes[7] = Convert.ToByte(str1[4]);
          break;
        case 3:
          length = StramatelWaterSender._TemplateX56.Length;
          Array.Copy((Array) StramatelWaterSender._TemplateX56, 0, (Array) _send_bytes, 0, length);
          string str2 = GetFieldValue("GameTime").Trim();
          if (!str2.Contains(".") && !str2.Contains(":"))
            str2 = "0:" + str2.Trim().PadLeft(2, '0');
          if (str2.Contains(":"))
            str2 = str2.PadLeft(5);
          else if (str2.Contains("."))
            str2 = str2.Trim() == "0.0" || _redlight ? " 0.00" : str2.Trim().PadLeft(4, '0').PadRight(5);
          _send_bytes[4] = Convert.ToByte(str2[0]);
          _send_bytes[5] = Convert.ToByte(str2[1]);
          _send_bytes[6] = Convert.ToByte(str2[3]);
          _send_bytes[7] = Convert.ToByte(str2[4]);
          string str3 = GetStringMinSize("ScoreHome", 1).PadLeft(2);
          _send_bytes[8] = (byte) str3[0];
          _send_bytes[9] = (byte) str3[1];
          string str4 = GetStringMinSize("ScoreGuest", 1).PadLeft(2);
          _send_bytes[12] = (byte) str4[0];
          _send_bytes[13] = (byte) str4[1];
          _send_bytes[14] = (byte) GetStringMinSize("Period", 1)[0];
          _send_bytes[17] = (byte) GetStringMinSize("TimeOutsHome", 1).PadLeft(1)[0];
          _send_bytes[18] = (byte) GetStringMinSize("TimeOutsGuest", 1).PadLeft(1)[0];
          _send_bytes[19] = Form1.UseBasketHorn >= 1 ? (_horn ? (byte) 49 : (byte) 48) : (byte) 48;
          break;
      }
      return length;
    }

    private void _make_telegram_erase_player_name()
    {
      for (int index = 2; index < 53; ++index)
        _send_bytes[index] = (byte) 32;
      _send_bytes[1] = (byte) 116;
      _send_bytes[4] = (byte) 116;
      _send_bytes[5] = (byte) 116;
    }

    private void _make_telegram_team_name()
    {
      for (int index = 2; index < 53; ++index)
        _send_bytes[index] = (byte) 32;
      _send_bytes[1] = (byte) 98;
      string str1 = GetFieldValue("TeamNameHome").ToUpper().Trim().Replace("Ä", "AE").Replace("Ö", "OE").Replace("Ü", "UE").Replace("ß", "SS");
      if (str1.Length < 8)
        str1 = " " + str1;
      string str2 = str1.PadRight(9).Substring(0, 9);
      string str3 = GetFieldValue("TeamNameGuest").Trim().Replace("Ä", "AE").Replace("Ö", "OE").Replace("Ü", "UE").Replace("ß", "SS");
      if (str3.Length < 8)
        str3 += " ";
      string str4 = str3.PadLeft(9).Substring(0, 9);
      if (_version == 2)
      {
        int num = 0;
        for (int index = 0; index < 9; ++index)
        {
          _send_bytes[num + 6] = (byte) 0;
          _send_bytes[num + 7] = (byte) str2[index];
          _send_bytes[num + 30] = (byte) 0;
          _send_bytes[num + 31] = (byte) str4[index];
          num += 2;
        }
      }
      else
      {
        for (int index = 0; index < 9; ++index)
        {
          _send_bytes[index + 6] = (byte) str2[index];
          _send_bytes[index + 18] = (byte) str4[index];
        }
      }
      _send_bytes[53] = (byte) 13;
    }

    private void _make_telegram_team_name(HomeGuestTeam Team)
    {
      for (int index = 2; index < 53; ++index)
        _send_bytes[index] = (byte) 32;
      _send_bytes[1] = (byte) 119;
      string str = GetFieldValue("TeamNameHome").PadRight(20).Substring(0, 20);
      if (Team == HomeGuestTeam.Guest)
      {
        _send_bytes[1] = (byte) 98;
        str = GetFieldValue("TeamNameGuest").PadRight(20).Substring(0, 20);
      }
      for (int index = 0; index < 20; ++index)
        _send_bytes[index + 6] = (byte) str[index];
      _send_bytes[53] = (byte) 13;
    }

    private void _make_telegram_player_name(HomeGuestTeam Team, int Index)
    {
      string str1 = "NameH";
      string str2 = "NoH";
      for (int index = 2; index < 53; ++index)
        _send_bytes[index] = (byte) 32;
      _send_bytes[1] = (byte) 119;
      if (Team == HomeGuestTeam.Guest)
      {
        str1 = "NameG";
        str2 = "NoG";
        _send_bytes[1] = (byte) 98;
      }
      string str3 = GetFieldValue(str2 + Index.ToString()).PadLeft(2);
      if (_version == 2)
      {
        _send_bytes[2] = (byte) (48 + Index);
        _send_bytes[22] = (byte) 0;
        _send_bytes[51] = (byte) str3[0];
        _send_bytes[52] = (byte) str3[1];
        if (str3[1] == '0' && str3[0] == ' ')
        {
          _send_bytes[51] = (byte) 48;
          _send_bytes[52] = (byte) 48;
        }
      }
      else
      {
        _send_bytes[4] = (byte) Index.ToString().PadLeft(2, '0')[0];
        _send_bytes[5] = (byte) Index.ToString().PadLeft(2, '0')[1];
        string str4 = GetFieldValue(str1 + Index.ToString()).ToUpper().Trim().Replace("Ä", "AE").Replace("Ö", "OE").Replace("Ü", "UE").Replace("ß", "SS").PadLeft(10).Substring(0, 10);
        for (int index = 0; index < 10; ++index)
          _send_bytes[index + 6] = (byte) str4[index];
        _send_bytes[16] = (byte) str3[0];
        _send_bytes[17] = (byte) str3[1];
      }
      _send_bytes[53] = (byte) 13;
    }

    private string GetStringMinSize(string DBFieldName, int MinNoOfBytes)
    {
      string fieldValue = GetFieldValue(DBFieldName);
      while (fieldValue.Length < MinNoOfBytes)
        fieldValue += " ";
      return fieldValue;
    }

    private int _make_telegram_intermediate()
    {
      _TempSize = 0;
      switch (_StateMachineState)
      {
        case 0:
          _TempSize = StramatelWaterSender._Intermediate0.Length;
          Array.Copy((Array) StramatelWaterSender._Intermediate0, 0, (Array) _send_bytes, 0, _TempSize);
          break;
        case 2:
          _TempSize = StramatelWaterSender._Intermediate2.Length;
          Array.Copy((Array) StramatelWaterSender._Intermediate2, 0, (Array) _send_bytes, 0, _TempSize);
          break;
      }
      return _TempSize;
    }

    private int _make_telegram_volley()
    {
      int length = _send_bytes.Length;
      switch (_StateMachineState)
      {
        case 1:
          length = StramatelWaterSender._TemplateX4D.Length;
          Array.Copy((Array) StramatelWaterSender._TemplateX4D, 0, (Array) _send_bytes, 0, length);
          string str1 = GetStringMinSize("ScoreHome", 1).PadLeft(2);
          _send_bytes[4] = (byte) str1[0];
          _send_bytes[5] = (byte) str1[1];
          string str2 = GetStringMinSize("ScoreGuest", 1).PadLeft(2);
          _send_bytes[6] = (byte) str2[0];
          _send_bytes[7] = (byte) str2[1];
          break;
        case 3:
          length = StramatelWaterSender._TemplateX56.Length;
          Array.Copy((Array) StramatelWaterSender._TemplateX56, 0, (Array) _send_bytes, 0, length);
          string str3 = GetStringMinSize("ScoreHome", 1).PadLeft(2);
          _send_bytes[4] = (byte) str3[0];
          _send_bytes[5] = (byte) str3[1];
          string str4 = GetStringMinSize("ScoreGuest", 1).PadLeft(2);
          _send_bytes[6] = (byte) str4[0];
          _send_bytes[7] = (byte) str4[1];
          _send_bytes[14] = (byte) GetStringMinSize("Period", 1)[0];
          _send_bytes[8] = (byte) 48;
          _send_bytes[9] = (byte) GetStringMinSize("SetsWonHome", 1).PadLeft(1)[0];
          _send_bytes[12] = (byte) 48;
          _send_bytes[13] = (byte) GetStringMinSize("SetsWonGuest", 1).PadLeft(1)[0];
          _send_bytes[17] = (byte) GetStringMinSize("TimeOutsHome", 1).PadLeft(1)[0];
          _send_bytes[18] = (byte) GetStringMinSize("TimeOutsGuest", 1).PadLeft(1)[0];
          _send_bytes[19] = _horn ? (byte) 49 : (byte) 48;
          break;
      }
      return length;
    }

    private void _make_telegram_simple_timer()
    {
      _insert_gametime();
      _send_bytes[1] = (byte) 154;
      _send_bytes[19] = Form1.UseBasketHorn >= 1 ? (_horn ? (byte) 49 : (byte) 48) : (byte) 48;
      _send_bytes[21] = _time_running ? (byte) 49 : (byte) 48;
    }

    private enum SportCode
    {
      Basket_with_individual_Fouls = 51, // 0x00000033
      Handball_Soccer = 53, // 0x00000035
      Volleyball = 54, // 0x00000036
      Basket_with_individual_GuestPoints = 55, // 0x00000037
      Basket_with_individual_HomePoints = 56, // 0x00000038
      Tennis = 57, // 0x00000039
      TableTennis = 58, // 0x0000003A
      Guest_Team = 98, // 0x00000062
      LED_Test = 102, // 0x00000066
      Badminton = 108, // 0x0000006C
      ErasePlayerNames = 116, // 0x00000074
      Home_Team = 119, // 0x00000077
      ClockSet = 153, // 0x00000099
      SimpleTimer = 154, // 0x0000009A
      Training = 156, // 0x0000009C
    }
  }
}
