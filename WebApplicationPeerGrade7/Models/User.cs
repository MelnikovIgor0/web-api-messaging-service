using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace WebApplicationPeerGrade7.Models
{
    /// <summary>
    /// Класс пользователя.
    /// </summary>
    [DataContract]
    public class User : IComparable
    {
        /// <summary>
        /// Имя пользователя.
        /// </summary>
        [DataMember]
        public string UserName { get; set; }

        /// <summary>
        /// Почтовый ящик пользователя.
        /// </summary>
        [DataMember]
        public string UserEmail { get; set; }

        /// <summary>
        /// Конструктор пользователя.
        /// </summary>
        /// <param name="name">Имя пользователя.</param>
        /// <param name="email">Почтовый ящик пользователя.</param>
        public User(string name, string email)
        {
            UserName = name;
            UserEmail = email;
        }

        /// <summary>
        /// Перегруженное приведение к строке.
        /// </summary>
        /// <returns>Строковое значение пользователя.</returns>
        public override string ToString() => $"Username: {UserName}; Email: {UserEmail}";

        /// <summary>
        /// Компоратор пользователей (сравнение по почтовому ящику).
        /// </summary>
        /// <param name="obj">Объект, который должен быть пользователем.</param>
        /// <returns>Чиселку, как и любой компоратор.</returns>
        int IComparable.CompareTo(object obj)
        {
            return (this.UserEmail.CompareTo(((User)obj).UserEmail));
        }
    }
}
