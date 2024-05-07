using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows.Forms;
using System.IO;
using System.ComponentModel;

namespace knihovna
{
    internal class SQLClass
    {
        public static string Cs { get; private set; }
        public static SQLiteConnection Connection { get; private set; }// zde je označení serveru, označení databáze a autentikace uživatele
        private SQLiteCommand prikaz = new SQLiteCommand(Connection);
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
        public static string[] FindZakaznik(string jmeno, string prijmeni, string ZakaznikID)
        {
            try
            {
                SQLiteCommand prikaz = new SQLiteCommand(Connection);
                string command = "";
                if (jmeno != "")
                {
                    command += " jmeno='" + jmeno + "'";
                }
                if(prijmeni != "")
                {
                    if (command.Length > 3)
                    {
                        command += " AND ";
                    }
                    command += " prijmeni='" + prijmeni + "'";
                }
                if (ZakaznikID != "")
                {
                    if (command.Length > 3)
                    {
                        command += " AND ";
                    }
                    command += " ZakaznikID='" + ZakaznikID + "'";
                }
                Connect();
                string[] exitString = new string[3];
                prikaz.CommandText = "SELECT * FROM zakaznici WHERE" + command+";";
                using (var reader = prikaz.ExecuteReader()) { while (reader.Read()) {exitString[0] = (string)reader["jmeno"]; exitString[1] = (string)reader["prijmeni"]; exitString[2] = Convert.ToInt64(reader["ZakaznikID"]).ToString();} }
                Disconnect();

                return exitString;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message);return null; }
        }
        public static void KnihaZakaznikEdit(int KnihaID, int ZakaznikID)
        {
            try
            {
                Connect();
                SQLiteCommand prikaz = new SQLiteCommand(Connection);
                prikaz.CommandText = "UPDATE knihy SET ZakaznikID = " + ZakaznikID + " WHERE KnihaID = " + KnihaID + ";";
                prikaz.ExecuteNonQuery();
                Disconnect();
            }catch(Exception ex) { MessageBox.Show(ex.Message); }
        }
        public static BindingList<Kniha> FindKniha(int ZakaznikID)
        {
            BindingList<Kniha> list = new BindingList<Kniha>();
            SQLiteCommand prikaz = new SQLiteCommand(Connection);
            Connect();  
            prikaz.CommandText = "SELECT * FROM knihy WHERE ZakaznikID = 1;";
            using (var reader = prikaz.ExecuteReader()) { 
                while (reader.Read()) {
                    list.Add(new Kniha(Convert.ToInt32(reader["KnihaID"]), (string)reader["nazev"], Convert.ToInt32(reader["AutorID"]), Convert.ToInt32(reader["ZanrID"]), (bool)reader["pujcena"], Convert.ToInt32(reader["ZakaznikID"]) ));
                }
            Disconnect();
            return list;
            }
        }
        public static void VytvorDatabazi()
        {
            try
            {
                Connect();
                SQLiteCommand prikaz = new SQLiteCommand(Connection);

                prikaz.CommandText = "DROP TABLE IF EXISTS knihy;";//id, nazev, id_autora, id_zanr, pujcena, id_zakaznik(pokud je půjčena)
                prikaz.ExecuteNonQuery();
                prikaz.CommandText = "CREATE TABLE knihy(KnihaID INTEGER PRIMARY KEY AUTOINCREMENT, nazev VARCHAR(15), AutorID INTEGER, ZanrID INTEGER, pujcena BOOLEAN, ZakaznikID INTEGER);";
                prikaz.ExecuteNonQuery();
                prikaz.CommandText = "INSERT INTO knihy(nazev, AutorID, ZanrID, pujcena, ZakaznikID) VALUES('Zaklinac', 1, 1, false, 1);";
                prikaz.ExecuteNonQuery();
                prikaz.CommandText = "INSERT INTO knihy(nazev, AutorID, ZanrID, pujcena, ZakaznikID) VALUES('Harry Potter a kamen mudrcu', 2, 1, false, 1);";
                prikaz.ExecuteNonQuery();
                prikaz.CommandText = "INSERT INTO knihy(nazev, AutorID, ZanrID, pujcena, ZakaznikID) VALUES('1984', 3, 1, false, 2);";
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
                prikaz.CommandText = "INSERT INTO zanr(nazev) VALUES('Fantasy');";
                prikaz.ExecuteNonQuery();
                prikaz.CommandText = "INSERT INTO zanr(nazev) VALUES('Roman');";
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
                prikaz.CommandText = "INSERT INTO zakaznici(jmeno, prijmeni) VALUES('Jakub', 'Mech');";
                prikaz.ExecuteNonQuery();
                prikaz.CommandText = "INSERT INTO zakaznici(jmeno, prijmeni) VALUES('Dorothea', 'Knezourkova');"; //ta mě cancelne na twitteru
                prikaz.ExecuteNonQuery();

                Disconnect();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}