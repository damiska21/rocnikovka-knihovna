using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows.Forms;
using System.IO;
using System.ComponentModel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Runtime.Remoting.Messaging;

namespace knihovna
{
    internal class SQLClass
    {
        public static string Cs { get; private set; }
        public static SQLiteConnection Connection { get; private set; }// zde je označení serveru, označení databáze a autentikace uživatele
        private SQLiteCommand prikaz;
        public SQLClass()
        {
            Cs = @"URI=file:../../../knihovna.db"; //cesta k db souboru
            Connection = new SQLiteConnection(Cs);
        }
        //stránky - půjčit/vrátit knihu
        //          přidat knihu/autora/žánr/zákazníka
        //          zobrazení/edit
        //     DONE     vytvoření db
        public static void Connect()
        {
            try
            {
                Connection.Open();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        public static void Disconnect() {
            try
            {
                Connection.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        #region NEW A CHANGE
        public static void KnihaZakaznikEdit(int KnihaID, int ZakaznikID, string date)
        {
            try
            {
                Connect();
                SQLiteCommand prikaz = new SQLiteCommand(Connection);
                prikaz.CommandText = "UPDATE knihy SET ZakaznikID = " + ZakaznikID + " WHERE KnihaID = " + KnihaID + ";";
                prikaz.ExecuteNonQuery();
                if (ZakaznikID != -1)
                {
                    prikaz.CommandText = "UPDATE knihy SET pujcenoDo = '" + date + "' WHERE KnihaID = " + KnihaID + ";";
                    prikaz.ExecuteNonQuery();
                }
                Disconnect();
            }catch(Exception ex) { MessageBox.Show(ex.Message); }
        }
        
        public static void ZmenZakaznika(string jmeno, string prijmeni, int id)
        {
            try
            {
                Connect();
                SQLiteCommand prikaz = new SQLiteCommand(Connection);
                prikaz.CommandText = "UPDATE zakaznici SET jmeno = @jmeno, prijmeni = @prijmeni WHERE ZakaznikID = @ZakaznikID";
                prikaz.Parameters.AddWithValue("@jmeno", jmeno);
                prikaz.Parameters.AddWithValue("@prijmeni", prijmeni);
                prikaz.Parameters.AddWithValue("@ZakaznikID", id);
                prikaz.ExecuteNonQuery();
                Disconnect();
                MessageBox.Show("uživatel upraven");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message);}
        }
        public static void ZmenKnihu(string nazev, int AutorID, int KnihaID, int ZakaznikID, int ZanrID, string pujcenoDo)
        {
            try
            {
                Connect();
                SQLiteCommand prikaz = new SQLiteCommand(Connection);
                prikaz.CommandText = "UPDATE knihy SET AutorID = @AutorID, nazev = @nazev, ZanrID = @ZanrID, ZakaznikID = @ZakaznikID, pujcenoDo = @pujcenoDo WHERE KnihaID = @KnihaID";
                prikaz.Parameters.AddWithValue("@AutorID", AutorID);
                prikaz.Parameters.AddWithValue("@nazev", nazev);
                prikaz.Parameters.AddWithValue("@ZanrID", ZanrID);
                prikaz.Parameters.AddWithValue("@ZakaznikID", ZakaznikID);
                prikaz.Parameters.AddWithValue("@KnihaID", KnihaID);
                prikaz.Parameters.AddWithValue("@pujcenoDo", pujcenoDo);
                prikaz.ExecuteNonQuery();
                Disconnect();
                MessageBox.Show("Kniha upravena");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        public static void ZmenAutora(string jmeno, string prijmeni, int id)
        {
            try
            {
                Connect();
                SQLiteCommand prikaz = new SQLiteCommand(Connection);
                prikaz.CommandText = "UPDATE autori SET jmeno = @jmeno, prijmeni = @prijmeni WHERE AutorID = @AutorID";
                prikaz.Parameters.AddWithValue("@jmeno", jmeno);
                prikaz.Parameters.AddWithValue("@prijmeni", prijmeni);
                prikaz.Parameters.AddWithValue("@AutorID", id);
                prikaz.ExecuteNonQuery();
                Disconnect();
                MessageBox.Show("Autor upraven");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        public static void ZmenZanr(string nazev, int id)
        {
            try
            {
                Connect();
                SQLiteCommand prikaz = new SQLiteCommand(Connection);
                prikaz.CommandText = "UPDATE zanr SET nazev = @nazev WHERE ZanrID = @id";
                prikaz.Parameters.AddWithValue("@nazev", nazev);
                prikaz.Parameters.AddWithValue("@id", id);
                prikaz.ExecuteNonQuery();
                Disconnect();
                MessageBox.Show("Žánr upraven");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        public static void NewKniha(string nazev, int ZanrID, int AutorID)
        {
            try
            {
                Connect();
                SQLiteCommand prikaz = new SQLiteCommand(Connection);
                prikaz.CommandText = "INSERT INTO knihy(nazev, AutorID, ZanrID, ZakaznikID) VALUES(@nazev, @AutorID, @ZanrID, -1);";
                prikaz.Parameters.AddWithValue("@AutorID", AutorID);
                prikaz.Parameters.AddWithValue("@nazev", nazev);
                prikaz.Parameters.AddWithValue("@ZanrID", ZanrID);
                prikaz.ExecuteNonQuery();
                Disconnect();
                MessageBox.Show("Kniha přidána");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        public static void NewAutor(string jmeno, string prijmeni)
        {
            try
            {
                Connect();
                SQLiteCommand prikaz = new SQLiteCommand(Connection);
                prikaz.CommandText = "INSERT INTO autori(jmeno, prijmeni) VALUES(@jmeno, @prijmeni);";
                prikaz.Parameters.AddWithValue("@jmeno", jmeno);
                prikaz.Parameters.AddWithValue("@prijmeni", prijmeni);
                prikaz.ExecuteNonQuery();
                Disconnect();
                MessageBox.Show("Autor přidán!");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        public static void NewZakaznik(string jmeno, string prijmeni)
        {
            try
            {
                Connect();
                SQLiteCommand prikaz = new SQLiteCommand(Connection);
                prikaz.CommandText = "INSERT INTO zakaznici(jmeno, prijmeni) VALUES(@jmeno, @prijmeni);";
                prikaz.Parameters.AddWithValue("@jmeno", jmeno);
                prikaz.Parameters.AddWithValue("@prijmeni", prijmeni);
                prikaz.ExecuteNonQuery();
                Disconnect();
                MessageBox.Show("Zákazník přidán!");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        public static void NewZanr(string nazev)
        {
            try
            {
                Connect();
                SQLiteCommand prikaz = new SQLiteCommand(Connection);
                prikaz.CommandText = "INSERT INTO zanr(nazev) VALUES(@nazev);";
                prikaz.Parameters.AddWithValue("@nazev", nazev);
                prikaz.ExecuteNonQuery();
                Disconnect();
                MessageBox.Show("Žánr přidán!");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        #endregion

        #region SELECT A FIND
        public static string[] FindZakaznik(string jmeno, string prijmeni, string ZakaznikID)
        {
            try
            {
                SQLiteCommand prikaz = new SQLiteCommand(Connection);
                prikaz.CommandText = "SELECT * FROM zakaznici WHERE";
                if (jmeno != "")
                {
                    prikaz.CommandText += " like('%"+jmeno+"%', jmeno)";
                }
                if (prijmeni != "")
                {
                    if (prikaz.CommandText.Length > 30)
                    {
                        prikaz.CommandText += " AND ";
                    }
                    prikaz.CommandText += " like('%"+prijmeni+"%', prijmeni)";
                }
                if (ZakaznikID != "")
                {
                    if (prikaz.CommandText.Length > 30)
                    {
                        prikaz.CommandText += " AND ";
                    }
                    prikaz.CommandText += " ZakaznikID="+ZakaznikID;
                }
                Connect();
                string[] exitString = new string[3];
                using (var reader = prikaz.ExecuteReader()) { while (reader.Read()) { exitString[0] = (string)reader["jmeno"]; exitString[1] = (string)reader["prijmeni"]; exitString[2] = Convert.ToInt64(reader["ZakaznikID"]).ToString(); } }
                Disconnect();

                return exitString;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); return null; }
        }
        public static int FindAutorByName(string jmeno)
        {
            try
            {
                Connect();
                SQLiteCommand prikaz = new SQLiteCommand(Connection);
                string command = "";
                int a = -1;
                if (jmeno != "")
                {
                    command += "WHERE like('%"+jmeno+ "%', jmeno) OR like('%" + jmeno + "%', prijmeni);";
                    prikaz.CommandText = "SELECT * FROM autori " + command;
                    using (var reader = prikaz.ExecuteReader()) { while (reader.Read()) { a = Convert.ToInt32(reader["AutorID"]); } }
                }
                Disconnect();
                return a;
            }
            catch (Exception ex) { /*MessageBox.Show(ex.Message + "findautorbyname");*/ return -1; }
        }
        public static int FindZanrByName(string nazev)
        {
            try
            {
                Connect();
                SQLiteCommand prikaz = new SQLiteCommand(Connection);
                int a = -1;
                prikaz.CommandText = "SELECT * FROM zanr WHERE like('%"+ nazev +"%', nazev);";
                using (var reader = prikaz.ExecuteReader()) { while (reader.Read()) { a =  Convert.ToInt32(reader["ZanrID"]); } }
                Disconnect();
                return a;
            }
            catch (Exception ex) {/*MessageBox.Show(ex.Message);*/ return -1; }
        }
        
        public static void DeleteIndex(int id, string table)
        {
            try
            {
                Connect();
                SQLiteCommand prikaz = new SQLiteCommand(Connection);
                switch(table)
                {
                    case "zakaznici":
                        prikaz.CommandText = "DELETE FROM zakaznici WHERE ZakaznikID = @id;";
                        break;
                    case "knihy":
                        prikaz.CommandText = "DELETE FROM knihy WHERE KnihaID  = @id;";
                        break;
                    case "autori":
                        prikaz.CommandText = "DELETE FROM autori WHERE AutorID = @id;";
                        break;
                    case "zanr":
                        prikaz.CommandText = "DELETE FROM zanr WHERE ZanrID = @id;";
                        break;
                }
                prikaz.Parameters.AddWithValue("@id", id);
                prikaz.ExecuteNonQuery();
                MessageBox.Show("Položka úspěšně smazána!");
                Disconnect();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message);}
        }
        public static BindingList<Kniha> FindKniha(int KnihaID, string nazev, int AutorID, int ZanrID, int ZakaznikID)
        {
            BindingList<Kniha> list = new BindingList<Kniha>();
            SQLiteCommand prikaz = new SQLiteCommand(Connection);
            Connect();
            try
            {
                if (nazev == ""){nazev = "thats kinda sus not gonna lie";}
                prikaz.CommandText = "SELECT * FROM knihy WHERE KnihaID= @KnihaID OR like('%"+nazev+"%', nazev) OR AutorID= @AutorID OR ZanrID= @ZanrID OR ZakaznikID= @ZakaznikID;";
                prikaz.Parameters.AddWithValue("@KnihaID", KnihaID);
                prikaz.Parameters.AddWithValue("@AutorID", AutorID);
                prikaz.Parameters.AddWithValue("@ZanrID", ZanrID);
                prikaz.Parameters.AddWithValue("@ZakaznikID", ZakaznikID);
                if (KnihaID == -1 && nazev == "" && ZanrID == -1 && AutorID == -1 && ZakaznikID == -1)
                {
                    prikaz.CommandText = "SELECT * FROM knihy;"; //na display celé tabulky
                }
                using (var reader = prikaz.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Kniha(Convert.ToInt32(reader["KnihaID"]), (string)reader["nazev"], Convert.ToInt32(reader["AutorID"]), Convert.ToInt32(reader["ZanrID"]), Convert.ToInt32(reader["ZakaznikID"]), Convert.ToString(reader["pujcenoDo"])));
                    }
                    Disconnect();
                    
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }//{ MessageBox.Show("Žádná kniha nebyla nalezena");}
            return list;
        }
        public static BindingList<Zakaznik> ListZakaznik()
        {
            Connect();
            BindingList<Zakaznik> list = new BindingList<Zakaznik>();
            SQLiteCommand prikaz = new SQLiteCommand(Connection);
            prikaz.CommandText = "SELECT * FROM zakaznici";
            using (var reader = prikaz.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new Zakaznik(Convert.ToInt32(reader["ZakaznikID"]), (string)reader["jmeno"], (string)reader["prijmeni"]));
                }
                Disconnect();
            }
            return list;
        }
        public static BindingList<Autor> ListAutor()
        {
            Connect();
            BindingList<Autor> list = new BindingList<Autor>();
            SQLiteCommand prikaz = new SQLiteCommand(Connection);
            prikaz.CommandText = "SELECT * FROM autori";
            using (var reader = prikaz.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new Autor(Convert.ToInt32(reader["AutorID"]), (string)reader["jmeno"], (string)reader["prijmeni"]));
                }
                Disconnect();
            }
            return list;
        }
        public static BindingList<Zanr> ListZanr()
        {
            Connect();
            BindingList<Zanr> list = new BindingList<Zanr>();
            SQLiteCommand prikaz = new SQLiteCommand(Connection);
            prikaz.CommandText = "SELECT * FROM zanr";
            using (var reader = prikaz.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new Zanr(Convert.ToInt32(reader["ZanrID"]), (string)reader["nazev"]));
                }
                Disconnect();
            }
            return list;
        }
        #endregion
        
        #region VYTVOŘENÍ DB
        public static void VytvorDatabazi()
        {
            try
            {
                Connect();
                SQLiteCommand prikaz = new SQLiteCommand(Connection);

                prikaz.CommandText = "DROP TABLE IF EXISTS knihy;";//id, nazev, id_autora, id_zanr, id_zakaznik(pokud je půjčena)
                prikaz.ExecuteNonQuery();
                prikaz.CommandText = "CREATE TABLE knihy(KnihaID INTEGER PRIMARY KEY AUTOINCREMENT, nazev VARCHAR(15), AutorID INTEGER, ZanrID INTEGER, ZakaznikID INTEGER, pujcenoDo VARCHAR(12));";
                prikaz.ExecuteNonQuery();
                prikaz.CommandText = "INSERT INTO knihy(nazev, AutorID, ZanrID, ZakaznikID, pujcenoDo) VALUES('Zaklinac', 1, 1, 1, '2024-01-12');";
                prikaz.ExecuteNonQuery();
                prikaz.CommandText = "INSERT INTO knihy(nazev, AutorID, ZanrID, ZakaznikID, pujcenoDo) VALUES('Harry Potter a kamen mudrcu', 2, 1, 1, '2024-01-12');";
                prikaz.ExecuteNonQuery();
                prikaz.CommandText = "INSERT INTO knihy(nazev, AutorID, ZanrID, ZakaznikID, pujcenoDo) VALUES('1984', 3, 2,  2, '2024-04-12');";
                prikaz.ExecuteNonQuery();


                prikaz.CommandText = "DROP TABLE IF EXISTS autori;";//id, jmeno, prijmeni
                prikaz.ExecuteNonQuery();
                prikaz.CommandText = "CREATE TABLE autori(AutorID INTEGER PRIMARY KEY AUTOINCREMENT, jmeno VARCHAR(15), prijmeni VARCHAR(15));";
                prikaz.ExecuteNonQuery();
                prikaz.CommandText = "INSERT INTO autori(jmeno, prijmeni) VALUES('Andrzej', 'Sapkowski');";
                prikaz.ExecuteNonQuery();
                prikaz.CommandText = "INSERT INTO autori(jmeno, prijmeni) VALUES('J. K.', 'Rowling');";
                prikaz.ExecuteNonQuery();
                prikaz.CommandText = "INSERT INTO autori(jmeno, prijmeni) VALUES('George', 'Orwell');";
                prikaz.ExecuteNonQuery();


                prikaz.CommandText = "DROP TABLE IF EXISTS zanr;";//id, nazev
                prikaz.ExecuteNonQuery();
                prikaz.CommandText = "CREATE TABLE zanr(ZanrID INTEGER PRIMARY KEY AUTOINCREMENT, nazev VARCHAR(15));";
                prikaz.ExecuteNonQuery();
                prikaz.CommandText = "INSERT INTO zanr(nazev) VALUES('fantasy');";
                prikaz.ExecuteNonQuery();
                prikaz.CommandText = "INSERT INTO zanr(nazev) VALUES('roman');";
                prikaz.ExecuteNonQuery();


                prikaz.CommandText = "DROP TABLE IF EXISTS zakaznici;";//id, jmeno, prijmeni
                prikaz.ExecuteNonQuery();
                prikaz.CommandText = "CREATE TABLE zakaznici(ZakaznikID INTEGER PRIMARY KEY AUTOINCREMENT, jmeno VARCHAR(15), prijmeni VARCHAR(15));";
                prikaz.ExecuteNonQuery();
                prikaz.CommandText = "INSERT INTO zakaznici(jmeno, prijmeni) VALUES('Lukas', 'Lozler');";
                prikaz.ExecuteNonQuery();
                prikaz.CommandText = "INSERT INTO zakaznici(jmeno, prijmeni) VALUES('Jakub', 'Mech');";
                prikaz.ExecuteNonQuery();
                prikaz.CommandText = "INSERT INTO zakaznici(jmeno, prijmeni) VALUES('Martin', 'Varic');";
                prikaz.ExecuteNonQuery();
                prikaz.CommandText = "INSERT INTO zakaznici(jmeno, prijmeni) VALUES('Karel', 'Cerny');";
                prikaz.ExecuteNonQuery();
                prikaz.CommandText = "INSERT INTO zakaznici(jmeno, prijmeni) VALUES('Damian', 'Rohlicek');";
                prikaz.ExecuteNonQuery();
                prikaz.CommandText = "INSERT INTO zakaznici(jmeno, prijmeni) VALUES('Dorothea', 'Knezourkova');"; //ta mě cancelne na twitteru
                prikaz.ExecuteNonQuery();

                Disconnect();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        #endregion
    }
}