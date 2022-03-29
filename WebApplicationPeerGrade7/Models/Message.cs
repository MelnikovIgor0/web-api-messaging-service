using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace WebApplicationPeerGrade7.Models
{
    /// <summary>
    /// Класс сообщения.
    /// </summary>
    [DataContract]
    public class Message
    {
        /// <summary>
        /// Тема сообщения.
        /// </summary>
        [DataMember]
        public string Subject { get; set; }

        /// <summary>
        /// Текст сообщения.
        /// </summary>
        [DataMember]
        public string Mesage { get; set; }

        /// <summary>
        /// Идентификатор отправителя.
        /// </summary>
        [DataMember]
        public string SenderID { get; set; }

        /// <summary>
        /// Идентификатор получателя.
        /// </summary>
        [DataMember]
        public string ReceiverID { get; set; }

        /// <summary>
        /// Конструктор сообщения.
        /// </summary>
        /// <param name="subject">Тема сообщение.</param>
        /// <param name="message">Текст сообщения.</param>
        /// <param name="senderid">Идентификатор отправителя.</param>
        /// <param name="receiverid">Идентификатор получателя.</param>
        public Message(string subject, string message, string senderid, string receiverid)
        {
            Subject = subject;
            Mesage = message;
            SenderID = senderid;
            ReceiverID = receiverid;
        }

        /// <summary>
        /// Перегруженное приведение к строке.
        /// </summary>
        /// <returns>Строковое значение сообщения.</returns>
        public override string ToString() => $"Message from {SenderID} to {ReceiverID}; " +
            $"Subject: {Subject}; Message: {Mesage}";
    }
}
