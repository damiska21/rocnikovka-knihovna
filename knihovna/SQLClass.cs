﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows.Forms;
using System.IO;
using System.ComponentModel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
        public static int FindAutorByName(string jmeno, string prijmeni)
        {
            try
            {
                Connect();
                SQLiteCommand prikaz = new SQLiteCommand(Connection);
                string command = "";
                int a = -1;
                if (jmeno != "")
                {
                    command += " jmeno = '" + jmeno + "'";
                }
                if (prijmeni != "")
                {
                    if (command.Length > 3)
                    {
                        command += " AND ";
                    }
                    command += " prijmeni = '" + prijmeni + "'";
                }
                prikaz.CommandText = "SELECT * FROM autori WHERE"+command+";";
                using (var reader = prikaz.ExecuteReader()) { while (reader.Read()) { a =  Convert.ToInt32(reader["AutorID"]); } }
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
                prikaz.CommandText = "SELECT * FROM zanr WHERE nazev = '" + nazev + "';";
                using (var reader = prikaz.ExecuteReader()) { while (reader.Read()) { int a =  Convert.ToInt32(reader["ZanrID"]); } }
                Disconnect();
                return -1;
            }
            catch (Exception ex) { /*MessageBox.Show(ex.Message);*/ return -1; }
        }
        public static BindingList<Kniha> FindKniha(int KnihaID, string nazev, int AutorID, int ZanrID, int ZakaznikID)
        {
            BindingList<Kniha> list = new BindingList<Kniha>();
            SQLiteCommand prikaz = new SQLiteCommand(Connection);
            Connect();
            string command = "";
            if (KnihaID != -1)
            {
                command += " KnihaID='" + KnihaID + "'";
            }
            if (nazev != "")
            {
                if (command.Length > 3)
                {
                    command += " AND ";
                }
                command += " nazev='" + nazev + "'";
            }
            if (AutorID != -1)
            {
                if (command.Length > 3)
                {
                    command += " AND ";
                }
                command += " AutorID='" + AutorID + "'";
            }
            if (ZanrID != -1)
            {
                if (command.Length > 3)
                {
                    command += " AND ";
                }
                command += " ZanrID='" + ZanrID + "'";
            }
            if (ZakaznikID != -1)
            {
                if (command.Length > 3)
                {
                    command += " AND ";
                }
                command += " ZakaznikID='" + ZakaznikID + "'";
            }
            try
            {
                prikaz.CommandText = "SELECT * FROM knihy WHERE " + command + ";";
                using (var reader = prikaz.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Kniha(Convert.ToInt32(reader["KnihaID"]), (string)reader["nazev"], Convert.ToInt32(reader["AutorID"]), Convert.ToInt32(reader["ZanrID"]), Convert.ToInt32(reader["ZakaznikID"])));
                    }
                    Disconnect();
                    
                }
            }
            catch { MessageBox.Show("Žádná kniha nebyla nalezena");}
            return list;
        }
        public static void VytvorDatabazi()
        {
            try
            {
                Connect();
                SQLiteCommand prikaz = new SQLiteCommand(Connection);

                prikaz.CommandText = "DROP TABLE IF EXISTS knihy;";//id, nazev, id_autora, id_zanr, id_zakaznik(pokud je půjčena)
                prikaz.ExecuteNonQuery();
                prikaz.CommandText = "CREATE TABLE knihy(KnihaID INTEGER PRIMARY KEY AUTOINCREMENT, nazev VARCHAR(15), AutorID INTEGER, ZanrID INTEGER, ZakaznikID INTEGER);";
                prikaz.ExecuteNonQuery();
                prikaz.CommandText = "INSERT INTO knihy(nazev, AutorID, ZanrID, ZakaznikID) VALUES('Zaklinac', 1, 1, 1);";
                prikaz.ExecuteNonQuery();
                prikaz.CommandText = "INSERT INTO knihy(nazev, AutorID, ZanrID, ZakaznikID) VALUES('Harry Potter a kamen mudrcu', 2, 1, 1);";
                prikaz.ExecuteNonQuery();
                prikaz.CommandText = "INSERT INTO knihy(nazev, AutorID, ZanrID, ZakaznikID) VALUES('1984', 3, 1,  2);";
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
    }
}