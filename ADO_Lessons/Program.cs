using System;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;

namespace ADO_Lessons
{
    class Program
    {

        static void Main(string[] args)
        {
            // Режими работы. Присоединенный режим
            // Режимы работы - в зависимости от наличия активного (открытого) подключения к БД
            // - присоединенный: работает при открытом подключении, данные сразу попадают в БД
            // - отсоединенный: создается программный объект-буфер, в который считываются данные
            //   действия пользователя происходят с этим объектом (не с БД)
            //   в нужный момент производится синхронизация с БД

            // Задание 1. Создать таблицу для хранения чисел
            String connectionString = ConfigurationManager.ConnectionStrings["ADO_Lessons"].ConnectionString;
            var con = new SqlConnection(connectionString);
            try
            {
                con.Open();
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Connection OK");
            var cmd = new SqlCommand();
            cmd.Connection = con;
            //cmd.CommandText = "CREATE TABLE Numbers (id INT PRIMARY KEY IDENTITY (1,1), numb INT)";
            //try
            //{
            //    cmd.ExecuteNonQuery();
            //    Console.WriteLine("CREATED NEW");
            //}
            //catch (SqlException ex)
            //{
            //    Console.WriteLine("CREATED SKIPPED");
            //}

            // Задание 2. Добавить к таблице информацию о времени добавления числа
            //cmd.CommandText = "ALTER TABLE Numbers ADD moment DATETIME DEFAULT GETDATE()";
            //try
            //{
            //    cmd.ExecuteNonQuery();
            //    Console.WriteLine("ALTER NEW");
            //}
            //catch (SqlException ex)
            //{
            //    Console.WriteLine("ALTERED SKIPPED");
            //}

            // Задание 3. Внести в таблице 10 случайных чисел одним запросом

            //Random rand = new Random();
            //int[] acc = new int[10];
            //for (int i = 0; i < 10; i++)
            //{
            //    acc[i] = rand.Next()%100;
            //}
            //for (int i = 0; i < 10; i++)
            //{
            //    cmd.CommandText = $"INSERT INTO Numbers (numb) VALUES ({acc[i]})";
            //    try
            //    {
            //        cmd.ExecuteNonQuery();
            //        //Console.WriteLine("ALTER NEW");
            //    }
            //    catch (SqlException ex)
            //    {
            //        Console.WriteLine(ex.Message);
            //        Console.WriteLine("ALTERED SKIPPED");
            //    }
            //}
            //Вариант 2
            //String g;
            //var sb = new StringBuilder();
            //sb.Append("INSERT INTO Numbers (numb) VALUES ("+ acc[0] +")");
            //for (int i = 1; i < 10; i++)
            //{
            //    sb.Append(",("+ acc[i] +")");
            //}
            //g=sb.ToString();
            //Console.WriteLine(g);
            //Console.WriteLine("Press a key to exit");

            // Задание 4. Вывести среднее значение чисел
            //cmd.CommandText = "SELECT AVG(numb) FROM Numbers";
            //double avg;
            //try
            //{
            //    avg = Convert.ToDouble(cmd.ExecuteScalar());
            //    Console.WriteLine(avg);
            //}
            //catch (SqlException ex) //Исключение SQL - ошибка в запросе
            //{
            //    Console.WriteLine("SQL: {0} ", ex.Message);
            //}
            //catch (InvalidCastException ex) //Исключение в преобразовании (пришло НЕ ЧИСЛО)
            //{
            //    Console.WriteLine("NaN: {0} ", ex.Message);
            //}

            //Задание 5. Кол-во чисел
            //cmd.CommandText = "SELECT COUNT(numb) FROM Numbers";
            //int kol;
            //try
            //{
            //    kol = Convert.ToInt32(cmd.ExecuteScalar());
            //    Console.WriteLine(kol);
            //}
            //catch (SqlException ex) //Исключение SQL - ошибка в запросе
            //{
            //    Console.WriteLine("SQL: {0} ", ex.Message);
            //}
            //catch (InvalidCastException ex) //Исключение в преобразовании (пришло НЕ ЧИСЛО)
            //{
            //    Console.WriteLine("NaN: {0} ", ex.Message);
            //}

            // Задание 6. Вывести кол-во, среднее значение и последний момент добавления (одним запросом)
            cmd.CommandText = "SELECT COUNT(id), AVG(numb), MAX(moment) FROM Numbers";
            SqlDataReader reader;
            try
            {
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine("cnt: {0}, avg: {1}, max: {2}", 
                        reader.GetInt32(0), 
                        reader.GetValue(1), // Обобщенный метод для любого типа
                        reader.GetDateTime(2));
                }
            }
            catch (SqlException ex) //Исключение SQL - ошибка в запросе
            {
                Console.WriteLine("SQL: {0} ", ex.Message);
                Console.ReadKey();
                return;
            }
           
           
            Console.ReadKey();
            con.Close();
            con.Dispose();

        }
        static void Main2(string[] args)
        {
            // Для портируемости переносим connectionString в App.config
            // Для доступа к App.config применяется ConfigurationManager из сборки System.Configuration (проверить наличие ссылки)
            // Извлекаем

            String connectionString = ConfigurationManager.ConnectionStrings["ADO_Lessons"].ConnectionString;

            var con = new SqlConnection(connectionString);
            try
            {
                con.Open();
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Connection OK");

            // Читаем данные
            SqlDataReader reader;
            var cmd = new SqlCommand("SELECT * FROM Test", con);
            try
            {
                reader = cmd.ExecuteReader();
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
                return;
            }

            // SqlDataReader используется "построчно" - метод Read() загружает следующую строку
            while (reader.Read())
            {
                // После загрузки данные доступны геттерами (по типу данных) с индексом поля
                Console.WriteLine("{0} {1}", reader.GetInt32(0), reader.GetString(1));
            }
            Console.ReadKey();
        }
        static void Main1(string[] args)
        {
            //Взаимовдействие базы данных состоит из несколько этапов:

            //1. Подключение
            SqlConnection con; //ADO объект, ответсвенный за подключение к БД
            //Строка подключения - параметры для связи с СУБД
            String connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Users\MIKOLKA\ADO_Lessons\ADO_Lessons\ADO_Lessons\ADO_Lessons.mdf;Integrated Security=True";
            //Инициализируем объект подключение (не создает самого подключения)
            con = new SqlConnection(connectionString);
            //Подключаемся
            try
            {
                con.Open();
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Connection OK");

            //2. Выполнение команд
            //2.1. Заполнение
            SqlCommand cmd = new SqlCommand(); //Объект для обслуживание команды (SQL-выражения)
            cmd.Connection = con;
            cmd.CommandText = "CREATE TABLE Test (id INT PRIMARY Y, str NVARCHAR(32))"; //SQL-вырKEажения

            //2.2. Выполнение
            //Для выполнения есть несколько методов:
            // cmd.ExecuteNonQuery() - для команд DDL; INSERT, UPDATE, DELETE
            // cmd.ExecuteScalar() - для возврата одного значения (чаще всего для аггрегаторов)
            // cmd.ExecuteReader() - для возврата таблицы
            //Все методы выполнения могут бросать исключения - нужен блок try{} catch{}
            try
            {
                cmd.ExecuteNonQuery();
                Console.WriteLine("CREATED NEW");
            }
            catch (SqlException ex)
            {
                //Console.WriteLine(ex.Message);
                Console.WriteLine("CREATED SKIPPED");
            }

            //Объект cmd может использоватся и для другой команды
            cmd.CommandText = "INSERT INTO Test VALUES (1, 'String 1')";
            try
            {
                cmd.ExecuteNonQuery();
                Console.WriteLine("INSERT OK");
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("INSERT 1 SKIPPED");
            }

            //Задание: обеспечить ввод в базу данных того, что вводит пользователь в консоле
            //(пустая строка - выход)
            String str;
            Console.WriteLine("Вводите строки через <Enter>. Пустая строка - выход");
            str = Console.ReadLine();
            while (!str.Equals(""))
            {
                str = str.Replace("'", "''");
                cmd.CommandText = $"INSERT INTO Test VALUES (((SELECT MAX(id) FROM Test) + 1), N'{str}')";
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(cmd.CommandText);
                    Console.ReadKey();
                    return;
                }
                str = Console.ReadLine();
            }
            Console.WriteLine("Press a key to exit");

            Console.ReadKey();
            con.Close(); //Закрывает соединение, но не разрушает его
            con.Dispose(); //Разрушает соединение с БД
        }
    }
}

/*
 
    ADO / PDO / JDBC - технологии доступа к Базам Данных
    Промежуточное звено, посредник между языком (программой) и СУБД (сервером БД)
    + Универсальность (заменяемости)
    + Скрытие SQL - применение языковых инструкций (LINQ), Адаптеров и т.п.
    + Идеология Code First - БД строится из анализа кода (Entity)
 
*/
