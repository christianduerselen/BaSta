using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using BaSta.Model;

namespace BaSta.Protocol.Stramatel
{
    public class StramatelProtocolMessageParser : IStramatelMessageParser
    {
        private const int MessageSize = 54;

        private readonly Queue<byte> _queue = new(MessageSize);
        private readonly ConcurrentQueue<IStramatelMessage> _parsedMessages = new();
        private readonly Dictionary<byte, Func<byte[], IStramatelMessage>> _parseDictionary = new() 
        {
            { 0x33, StramatelMessage0x33.Parse },
            { 0x37, StramatelMessage0x37.Parse },
            { 0x38, StramatelMessage0x38.Parse }
        };

        public void Parse(params byte[] data)
        {
            // Make parse method thread tolerant by locking the byte-queue
            lock (_queue)
            {
                // Handle byte after byte
                foreach (byte dataByte in data)
                {
                    // If the queue is currently full, remove one item first
                    if (_queue.Count == MessageSize)
                        _queue.Dequeue();

                    // Add the new item
                    _queue.Enqueue(dataByte);

                    // A stramatel message is always 54 bytes so not-full queue means incomplete data and can be discarded
                    if (_queue.Count != MessageSize)
                        continue;

                    // A stramatel message should start with 0xF8 or 0xF9
                    byte peekByte = _queue.Peek();
                    if (peekByte != 0xF8 && peekByte != 0xF9)
                        continue;

                    // Extract the whole message data
                    byte[] messageData = _queue.ToArray();

                    // A stramatel message should end with 0x0D
                    if (messageData[^1] != 0x0D)
                        continue;

                    // The second byte contains the message type which must be supported
                    if (!_parseDictionary.TryGetValue(messageData[1], out var messageParser))
                        continue;

                    // Try to create message instance from type parser
                    try
                    {
                        _parsedMessages.Enqueue(messageParser.Invoke(messageData));
                    }
                    catch (StramatelMessageParseException exception)
                    {
                        // TODO
                    }
                }
            }
        }

        public int MessagesAvailable => _parsedMessages.Count;
        
        public bool TryDequeue(out IStramatelMessage message)
        {
            return _parsedMessages.TryDequeue(out message);
        }
    }

    public interface IStramatelMessage
    {
        void ApplyTo(IGame game);
    }

    public interface IStramatelGame
    {
        public TimeSpan GameTime { get; }

        public TimeSpan ShotClock { get; }

        public int Quarter { get; }
    }
}