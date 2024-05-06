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
        private SQLiteCommand prikaz;
        public SQLClass()
        {
            Cs = @"URI=file:../../knihovna.db"; //cesta k db souboru
            Connection = new SQLiteConnection(Cs);
        }
        //stránky - půjčit/vrátit knihu
        //          přidat knihu/autora/žánr/zákazníka
        //          zobrazení/edit
        //          vytvoření db
        public static void Pripoj()
        {
            try
            {
                Connection.Open();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        public static void Odpoj() {
            try
            {
                Connection.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        public static void VytvorDatabazi()
        {
            try
            {
                Pripoj();
                SQLiteCommand prikaz = new SQLiteCommand(Connection);

                prikaz.CommandText = "DROP TABLE IF EXISTS knihy";//id, nazev, id_autora, id_zanr, pujcena, id_zakaznik(pokud je půjčena)
                prikaz.ExecuteNonQuery();
                prikaz.CommandText = "CREATE TABLE knihy(KnihaID INTEGER PRIMARY KEY AUTOINCREMENT, nazev VARCHAR(15), AutorID INTEGER, ZanrID INTEGER, pujcena BOOLEAN, ZakaznikID INTEGER);";
                prikaz.ExecuteNonQuery();
                prikaz.CommandText = "INSERT INTO knihy(nazev, AutorID, ZanrID, pujcena, ZakaznikID) VALUES('Zaklinac', 1, 1, false, -1);";
                prikaz.ExecuteNonQuery();


                prikaz.CommandText = "DROP TABLE IF EXISTS autori";//id, jmeno, prijmeni
                prikaz.ExecuteNonQuery();
                prikaz.CommandText = "CREATE TABLE autori(AutorID INTEGER PRIMARY KEY AUTOINCREMENT, jmeno VARCHAR(15), prijmeni VARCHAR(15));";
                prikaz.ExecuteNonQuery();
                prikaz.CommandText = "INSERT INTO autori(jmeno, prijmeni) VALUES('Andrzej', 'Sapkowski');";
                prikaz.ExecuteNonQuery();


                prikaz.CommandText = "DROP TABLE IF EXISTS zanr";//id, nazev
                prikaz.ExecuteNonQuery();
                prikaz.CommandText = "CREATE TABLE zanr(ZanrID INTEGER PRIMARY KEY AUTOINCREMENT, nazev VARCHAR(15));";
                prikaz.ExecuteNonQuery();
                prikaz.CommandText = "INSERT INTO zanr(nazev) VALUES('Fantasy');";
                prikaz.ExecuteNonQuery();


                prikaz.CommandText = "DROP TABLE IF EXISTS zakaznici";//id, jmeno, prijmeni
                prikaz.ExecuteNonQuery();
                prikaz.CommandText = "CREATE TABLE zakaznici(ZakaznikID INTEGER PRIMARY KEY AUTOINCREMENT, jmeno VARCHAR(15), prijmeni VARCHAR(15));";
                prikaz.ExecuteNonQuery();

                Odpoj();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}