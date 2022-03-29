using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using WebApplicationPeerGrade7.Models;

namespace WebApplicationPeerGrade7.Controllers
{
    /// <summary>
    /// Основной класс контроллера, тут вся магия.
    /// </summary>
    [ApiController]
    public class MainController : Controller
    {
        [DataMember]
        public List<User> users = new();
        
        [DataMember]
        public List<Message> messages = new();
        
        private static Random rnd = new Random();
        
        private const string allowedSymbols = "abcdefrghijklmnopqrstuvwxyz1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ. ";

        private const string pathUsers = "users.json";

        private const string pathMessages = "messages.json";

        /// <summary>
        /// Метод генерирует случайную строку.
        /// </summary>
        /// <param name="length">Длина строки.</param>
        /// <returns>Сгенерированная строка.</returns>
        private string GenerateRandomString(int length)
        {
            StringBuilder generatedString = new("");
            for (int i = 0; i < length; i++)
                generatedString.Append(allowedSymbols[rnd.Next(0, allowedSymbols.Length)]);
            return generatedString.ToString();
        }

        /// <summary>
        /// Метод генерирует случайное имя пользователя.
        /// </summary>
        /// <param name="length">Длина имени.</param>
        /// <returns>Сгенерированное имя.</returns>
        private string GenerateRandomName(int length)
        {
            StringBuilder generatedName = new("");
            generatedName.Append(char.ToUpper(allowedSymbols[rnd.Next(0, 26)]));
            for (int i = 0; i < length - 1; i++)
                generatedName.Append(allowedSymbols[rnd.Next(0, 26)]);
            return generatedName.ToString();
        }

        /// <summary>
        /// Метод генерирует случайный почтовый адрес.
        /// </summary>
        /// <param name="length">Длина именной части почтового адреса.</param>
        /// <returns>Сгенерированный почтовый адрес.</returns>
        private string GenerateRandomEmail(int length)
        {
            StringBuilder generatedEmail = new("");
            for (int i = 0; i < length; i++)
                generatedEmail.Append(allowedSymbols[rnd.Next(0, 36)]);
            switch (rnd.Next(0, 3))
            {
                case 0:
                    generatedEmail.Append("@gmail.com");
                    break;
                case 1:
                    generatedEmail.Append("@mail.ru");
                    break;
                case 2:
                    generatedEmail.Append("@edu.hse.ru");
                    break;
            }
            return generatedEmail.ToString();
        }

        /// <summary>
        /// Метод записывает список пользователей и сообщений в соответствующие файлы.
        /// </summary>
        /// <param name="path1">Путь к файлу с пользователями.</param>
        /// <param name="path2">Путь к файлу с сообщениями.</param>
        private void SaveData(string path1, string path2)
        {
            try
            {
                var fs = new FileStream(path1, FileMode.Create);
                var formatter = new DataContractJsonSerializer(typeof(List<User>));
                formatter.WriteObject(fs, users);
                fs.Close();
            }
            catch (Exception)
            {
            }
            try
            {
                var fs = new FileStream(path2, FileMode.Create);
                var formatter = new DataContractJsonSerializer(typeof(List<Message>));
                formatter.WriteObject(fs, messages);
                fs.Close();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Метод загружает данные из файла.
        /// </summary>
        /// <param name="path1">Путь к файлу с пользователями.</param>
        /// <param name="path2">Путь к файлу с сообщениями.</param>
        private void LoadData(string path1, string path2)
        {
            try
            {
                var fs = new FileStream(path1, FileMode.Open, FileAccess.Read);
                var formatter = new DataContractJsonSerializer(typeof(List<User>));
                users = (List<User>)formatter.ReadObject(fs);
            }
            catch (Exception)
            {
            }
            try
            {
                var fs = new FileStream(path2, FileMode.Open, FileAccess.Read);
                var formatter = new DataContractJsonSerializer(typeof(List<Message>));
                messages = (List<Message>)formatter.ReadObject(fs);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Метод генерирует пользователей и сообщения.
        /// </summary>
        /// <returns>Кортеж из списка сгенерированных пользователей и списка сгенерированных сообщений.</returns>
        [HttpPost("generate")]
        public IActionResult GenerateData()
        {
            int amountUsers = rnd.Next(5, 8);
            int amountMessages = rnd.Next(20, 51);
            for (int i = 0; i < amountUsers; i++)
            {
                string email = "";
                while (true)
                {
                    email = GenerateRandomEmail(rnd.Next(8, 16));
                    bool was = false;
                    foreach (User u in users) 
                        if (u.UserEmail == email)
                        {
                            was = true;
                            break;
                        }
                    if (!was) break;
                }
                users.Add(new User(GenerateRandomName(rnd.Next(3, 9)), email));
            }
            for (int i = 0; i < amountMessages; i++)
            {
                User from = users[0], to = users[0];
                while (from.UserEmail == to.UserEmail)
                {
                    from = users[rnd.Next(0, users.Count)];
                    to = users[rnd.Next(0, users.Count)];
                }
                messages.Add(new Message(GenerateRandomString(rnd.Next(5, 16)), 
                    GenerateRandomString(rnd.Next(10, 30)), from.UserEmail, to.UserEmail));
            }
            users.Sort();
            char sep = Path.DirectorySeparatorChar;
            SaveData(pathUsers, pathMessages);
            return Ok((users, messages));
        }

        /// <summary>
        /// Метод ищет пользователя по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор пользователя.</param>
        /// <returns>Результат поиска.</returns>
        [HttpGet("user/{id}")]
        public IActionResult GetUser(string id)
        {
            LoadData(pathUsers, pathMessages);
            foreach (User u in users)
                if (u.UserEmail == id)
                    return Ok(u);
            return NotFound();
        }

        /// <summary>
        /// Метод возвращает всех пользователей.
        /// </summary>
        /// <returns>Результат поиска.</returns>
        [HttpGet("users")]
        public IActionResult GetUsers()
        {
            LoadData(pathUsers, pathMessages);
            return Ok(users);
        }

        /// <summary>
        /// Метод ищет сообщения написанные одним указанным пользователем другому указанному пользователю.
        /// </summary>
        /// <param name="idFrom">Идентификатор отправителя сообщения.</param>
        /// <param name="idTo">Идентификатор получателя сообщения.</param>
        /// <returns>Результат поиска, список сообщений.</returns>
        [HttpGet("messages/{idFrom}-{idTo}")]
        public IActionResult GetMessagesFromFixedToFixed(string idFrom, string idTo)
        {
            LoadData(pathUsers, pathMessages);
            List<Message> neededMessages = new();
            foreach (Message m in messages)
                if (m.SenderID == idFrom && m.ReceiverID == idTo)
                    neededMessages.Add(m);
            return Ok(neededMessages);
        }

        /// <summary>
        /// Метод ищет все сообщения, написанные указанным пользователем.
        /// </summary>
        /// <param name="idSender">Идентификатор отправителя.</param>
        /// <returns>Результат поиска, список сообщений.</returns>
        [HttpGet("messages-from/{idSender}")]
        public IActionResult GetMessagesBySender(string idSender)
        {
            LoadData(pathUsers, pathMessages);
            List<Message> neededMessages = new();
            foreach (Message m in messages)
                if (m.SenderID == idSender)
                    neededMessages.Add(m);
            return Ok(neededMessages);
        }

        /// <summary>
        /// Метод ищет все сообщения, полученные указанным пользователем.
        /// </summary>
        /// <param name="idReceiver">Идентификатор получателя.</param>
        /// <returns>Результат поиска, список сообщений.</returns>
        [HttpGet("messages-to/{idReceiver}")]
        public IActionResult GetMessagesByReceiver(string idReceiver)
        {
            LoadData(pathUsers, pathMessages);
            List<Message> neededMessages = new();
            foreach (Message m in messages)
                if (m.ReceiverID == idReceiver)
                    neededMessages.Add(m);
            return Ok(neededMessages);
        }
    }
}
