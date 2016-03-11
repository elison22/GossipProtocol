using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace GossipProtocol.UserManagement
{
    public class UserManager
    {
        private string persistenceDir = "Data/";
        private string persistenceName = "users.json";
        //private string persistenceDir = @"Data\";
        //private string persistenceName = @"users.json";
        private static UserManager instance = null;
        private static List<User> users = null;
        private UserManager()
        {
            users = new List<User>();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static UserManager get()
        {
            if (instance == null)
            {
                Console.WriteLine("\nInstantiating UserManager");
                instance = new UserManager();
            }
            else
            {
                Console.WriteLine("\nUsing the existing UserManager with " + users.Count + " Users");
            }
            return instance;
        }
        
        private List<User> readUsers(string dir, string name)
        {
            string json = UserIO.readUserJson(dir, name);
            Console.WriteLine("\nRead in the following Json:\n" + json);

            List<User> readInUsers = JsonConvert.DeserializeObject<List<User>>(json) ?? new List<User>();
            Console.WriteLine("\nConverted it to " + readInUsers.Count + " Users.");

            return readInUsers;
        }
        private void writeUsers(string dir, string name)
        {
            Console.WriteLine("\nPreparing to persist " + users.Count + " users.");
            List<User> persisted = readUsers(dir, name);
            Console.WriteLine("\nFound " + persisted.Count + " users on disk.");
            users = combineLists(persisted, users);
            Console.WriteLine("\nCombined groups into " + users.Count +" users");
            string json = JsonConvert.SerializeObject(users);
            Console.WriteLine("\nSerialized users for writing into the following Json:\n" + json);
            UserIO.writeUserJson(dir, name, json);
        }

        // === Getter type things ===
        public User getUser(Guid id)
        {
            User match = users.FirstOrDefault(x => x.Id == id);
            return match;
        }

        public User getUser(string username)
        {
            User match = users.FirstOrDefault(x => x.UserName == username);
            return match;
        }

        public User getShortUser(string ShortId)
        {
            return (
                from u in users
                where u.ShortId == ShortId
                select u).FirstOrDefault();
        }

        public List<User> getUsers()
        {
            return users;
        }

        // === Setter/Adder type things ===
        public void addUser(User newUser)
        {
            users.Add(newUser);
            writeUsers(persistenceDir, persistenceName);
        }

        public void deleteUser(string username)
        {
            List<User> persisted = readUsers(persistenceDir, persistenceName);
            
            users = combineLists(persisted, users);
            users = (
                from u in users
                where u.UserName != username
                select u).ToList();

            string json = JsonConvert.SerializeObject(users);
            Console.WriteLine("\nSerialized users for writing into the following Json:\n" + json);
            UserIO.writeUserJson(persistenceDir, persistenceName, json);
        }

        private List<User> combineLists(List<User> fromFile, List<User> inMemory)
        {
            foreach(User memUser in inMemory)
            {
                if (fromFile.Contains(memUser))
                    continue;
                User usernameMatchInFile = (
                    from fileUser in fromFile
                    where fileUser.UserName == memUser.UserName
                    select fileUser).FirstOrDefault();
                if (usernameMatchInFile == null)
                    fromFile.Add(memUser);
                //else if (memUser.FS_Token != null && memUser.FS_Token != "")
                //    usernameMatchInFile.FS_Token = memUser.FS_Token;

                // TODO: add the reconciliation of neighbors among other things.
            }
            return fromFile;
        }

    }
}