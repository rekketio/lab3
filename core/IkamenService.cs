using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace core
{
    // ПРИМЕЧАНИЕ. Можно использовать команду "Переименовать" в меню "Рефакторинг", чтобы изменить имя интерфейса "IkamenService" в коде и файле конфигурации.
    [ServiceContract(CallbackContract = typeof(IServiceCallback))]
    public interface IkamenService
    {
        [OperationContract]
        Tuple<int, int, string> Connect(string nickname);
        [OperationContract]
        void Disconnect(int lobbyId);
        [OperationContract]
        void GameMoveSend(int GameSelection, int lobbyId, int senderID);
    }

    public interface IServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void StartGame(string nicknameOpponent);
        [OperationContract(IsOneWay = true)]
        void GameMoveReceive(int GameSelection);
    }
}
