// Copyright (c) 2008-2015, Hazelcast, Inc. All Rights Reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Hazelcast.Client.Protocol.Util;
using Hazelcast.IO;

namespace Hazelcast.Client.Protocol.Codec
{
    internal sealed class TransactionCommitCodec
    {
        public const int ResponseType = 100;
        public const bool Retryable = false;

        public static readonly TransactionMessageType RequestType = TransactionMessageType.TransactionCommit;

        public static ClientMessage EncodeRequest(string transactionId, long threadId, bool prepareAndCommit)
        {
            var requiredDataSize = RequestParameters.CalculateDataSize(transactionId, threadId, prepareAndCommit);
            var clientMessage = ClientMessage.CreateForEncode(requiredDataSize);
            clientMessage.SetMessageType((int) RequestType);
            clientMessage.SetRetryable(Retryable);
            clientMessage.Set(transactionId);
            clientMessage.Set(threadId);
            clientMessage.Set(prepareAndCommit);
            clientMessage.UpdateFrameLength();
            return clientMessage;
        }

        //************************ REQUEST *************************//

        public class RequestParameters
        {
            public static readonly TransactionMessageType TYPE = RequestType;
            public bool prepareAndCommit;
            public long threadId;
            public string transactionId;

            public static int CalculateDataSize(string transactionId, long threadId, bool prepareAndCommit)
            {
                var dataSize = ClientMessage.HeaderSize;
                dataSize += ParameterUtil.CalculateDataSize(transactionId);
                dataSize += Bits.LongSizeInBytes;
                dataSize += Bits.BooleanSizeInBytes;
                return dataSize;
            }
        }
    }
}