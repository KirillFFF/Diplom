using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProductsServer.Models;
using MySql.Data.MySqlClient;

namespace ProductsServer.DataBase
{
    public class ManageDataDB
    {
        private static volatile ManageDataDB _class;
        public static ManageDataDB Instance
        {
            get
            {
                return _class ??= new ManageDataDB();
                //if (_class == null)
                //{
                //    lock (_obj)
                //    {
                //        if (_class == null)
                //            _class = new ManageDataDB();
                //    }
                //}

                //return _class;
            }
        }

        public async Task<List<ArrivalProduct>> GetArrivalProductsAsync()
        {
            using MySqlCommand command = new();
            command.CommandText = "CALL GetArrivalProductsJSON";
            return JsonConvert.DeserializeObject<List<ArrivalProduct>>((await ProductsDB.Instance.GetDataAsync(command)).Rows[0][0].ToString());
        }
        public async Task<List<ArrivalProduct>> GetArrivalProductAsync(uint id)
        {
            return (await GetArrivalProductsAsync()).Where(x => x.Id.Equals(id)).ToList();
        }
        public async Task InsertArrivalProductAsync(ArrivalProduct product)
        {
            using MySqlCommand command = new();
            command.CommandText = "INSERT INTO `Приход товаров` VALUES (@id, @noteId, @productId, @count, @validUntil, @purchaseCost, @sellCost)";
            command.Parameters.AddWithValue("@id", product.Id);
            command.Parameters.AddWithValue("@noteId", product.Note.Id);
            command.Parameters.AddWithValue("@productId", product.Product.IdName);
            command.Parameters.AddWithValue("@count", product.Count);
            command.Parameters.AddWithValue("@validUntil", product.ValidUntil);
            command.Parameters.AddWithValue("@purchaseCost", product.PurchaseCost);
            command.Parameters.AddWithValue("@sellCost", product.SellCost);

            await ProductsDB.Instance.SetDataAsync(command);
        }
        public async Task UpdateArrivalProductAsync(uint id, ArrivalProduct product)
        {
            using MySqlCommand command = new();
            command.CommandText = "UPDATE `Приход товаров` SET `ТТН` = @noteId, `Товар` = @productId, `Количество` = @count, `Годен до` = @validUntil, `Цена закупки` = @purchaseCost, `Цена продажи` = @sellCost WHERE `Код` = @id";
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@noteId", product.Note.Id);
            command.Parameters.AddWithValue("@productId", product.Product.IdName);
            command.Parameters.AddWithValue("@count", product.Count);
            command.Parameters.AddWithValue("@validUntil", product.ValidUntil);
            command.Parameters.AddWithValue("@purchaseCost", product.PurchaseCost);
            command.Parameters.AddWithValue("@sellCost", product.SellCost);

            await ProductsDB.Instance.SetDataAsync(command);
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            using MySqlCommand command = new();
            command.CommandText = "CALL GetProductsJSON";
            return JsonConvert.DeserializeObject<List<Product>>((await ProductsDB.Instance.GetDataAsync(command)).Rows[0][0].ToString());
        }
        public async Task<List<Product>> GetProductAsync(string idName)
        {
            return (await GetProductsAsync()).Where(x => x.IdName.Equals(idName)).ToList();
        }
        public async Task InsertProductAsync(Product product)
        {
            using MySqlCommand command = new();
            command.CommandText = "INSERT INTO `Товары` VALUES(@idName, @name, @group, @unit, @manufacture)";
            command.Parameters.AddWithValue("@idName", product.IdName);
            command.Parameters.AddWithValue("@name", product.Name);
            command.Parameters.AddWithValue("@group", product.Group.Id);
            command.Parameters.AddWithValue("@unit", product.Unit.Id);
            command.Parameters.AddWithValue("@manufacture", product.Manufacture.Id);

            await ProductsDB.Instance.SetDataAsync(command);
        }
        public async Task UpdateProductAsync(string id, Product product)
        {
            using MySqlCommand command = new();
            command.CommandText = "UPDATE `Товары` SET `Наименование` = @name, `Товарная группа` = @group, `Единица измерения` = @unit, `Производитель` = @manufacture WHERE `Артикул` = @id";
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@name", product.Name);
            command.Parameters.AddWithValue("@group", product.Group.Id);
            command.Parameters.AddWithValue("@unit", product.Unit.Id);
            command.Parameters.AddWithValue("@manufacture", product.Manufacture.Id);

            await ProductsDB.Instance.SetDataAsync(command);
        }

        public async Task<List<NoteProduct>> GetNoteProductsAsync()
        {
            using MySqlCommand command = new();
            command.CommandText = "CALL GetNoteProductsJSON";
            return JsonConvert.DeserializeObject<List<NoteProduct>>((await ProductsDB.Instance.GetDataAsync(command)).Rows[0][0].ToString());
        }
        public async Task<List<NoteProduct>> GetNoteProductAsync(uint id)
        {
            return (await GetNoteProductsAsync()).Where(x => x.Id.Equals(id)).ToList();
        }
        public async Task InsertNoteProductAsync(NoteProduct note)
        {
            using MySqlCommand command = new();
            command.CommandText = "INSERT INTO `ТТН` VALUES (@id, @supplier, @employee, @date)";
            command.Parameters.AddWithValue("@id", note.Id);
            command.Parameters.AddWithValue("@supplier", note.Supplier.Id);
            command.Parameters.AddWithValue("@employee", note.Employee.Id);
            command.Parameters.AddWithValue("@date", note.Date);

            await ProductsDB.Instance.SetDataAsync(command);
        }
        public async Task UpdateNoteProductAsync(uint id, NoteProduct note)
        {
            using MySqlCommand command = new();
            command.CommandText = "UPDATE `ТТН` SET `Поставщик` = @supplier, `Сотрудник` = @employee, `Дата прихода` = @date WHERE `Код` = @id";
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@supplier", note.Supplier.Id);
            command.Parameters.AddWithValue("@employee", note.Employee.Id);
            command.Parameters.AddWithValue("@date", note.Date);

            await ProductsDB.Instance.SetDataAsync(command);
        }

        public async Task<List<ProductGroup>> GetProductGroupsAsync()
        {
            using MySqlCommand command = new();
            command.CommandText = "SELECT `Код` AS 'Id', `Наименование` AS 'Name' FROM `Товарные группы`";
            return JsonConvert.DeserializeObject<List<ProductGroup>>(JsonConvert.SerializeObject(await ProductsDB.Instance.GetDataAsync(command)));
        }
        public async Task InsertProductGroupAsync(ProductGroup group)
        {
            using MySqlCommand command = new();
            command.CommandText = "INSERT INTO `Товарные группы` VALUES(@id, @name)";
            command.Parameters.AddWithValue("@id", group.Id);
            command.Parameters.AddWithValue("@name", group.Name);

            await ProductsDB.Instance.SetDataAsync(command);
        }
        public async Task UpdateProductGroupAsync(uint id, ProductGroup group)
        {
            using MySqlCommand command = new();
            command.CommandText = "UPDATE `Товарные группы` SET `Наименование` = @name WHERE `Код` = @id";
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@name", group.Name);

            await ProductsDB.Instance.SetDataAsync(command);
        }

        public async Task<List<ProductUnit>> GetProductUnitsAsync()
        {
            using MySqlCommand command = new();
            command.CommandText = "SELECT `Код` AS 'Id', `Наименование` AS 'Name' FROM `Единицы измерения`";
            return JsonConvert.DeserializeObject<List<ProductUnit>>(JsonConvert.SerializeObject(await ProductsDB.Instance.GetDataAsync(command)));
        }
        public async Task InsertProductUnitAsync(ProductUnit unit)
        {
            using MySqlCommand command = new();
            command.CommandText = "INSERT INTO `Единицы измерения` VALUES(@id, @name)";
            command.Parameters.AddWithValue("@id", unit.Id);
            command.Parameters.AddWithValue("@name", unit.Name);

            await ProductsDB.Instance.SetDataAsync(command);
        }
        public async Task UpdateProductUnitAsync(uint id, ProductUnit unit)
        {
            using MySqlCommand command = new();
            command.CommandText = "UPDATE `Единицы измерения` SET `Наименование` = @name WHERE `Код` = @id";
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@name", unit.Name);

            await ProductsDB.Instance.SetDataAsync(command);
        }

        public async Task<List<Manufacture>> GetManufacturesAsync()
        {
            using MySqlCommand command = new();
            command.CommandText = "CALL GetManufacturesJSON";
            return JsonConvert.DeserializeObject<List<Manufacture>>((await ProductsDB.Instance.GetDataAsync(command)).Rows[0][0].ToString());
        }
        public async Task InsertManufactureAsync(Manufacture manufacture)
        {
            using MySqlCommand command = new();
            command.CommandText = "INSERT INTO `Производители` VALUES(@id, @name, @country)";
            command.Parameters.AddWithValue("@id", manufacture.Id);
            command.Parameters.AddWithValue("@name", manufacture.Name);
            command.Parameters.AddWithValue("@country", manufacture.Country.Id);

            await ProductsDB.Instance.SetDataAsync(command);
        }
        public async Task UpdateManufactureAsync(uint id, Manufacture manufacture)
        {
            using MySqlCommand command = new();
            command.CommandText = "UPDATE `Производители` SET `Наименование` = @name, `Страна` = @countryId WHERE `Код` = @id";
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@name", manufacture.Name);
            command.Parameters.AddWithValue("@countryId", manufacture.Country.Id);

            await ProductsDB.Instance.SetDataAsync(command);
        }

        public async Task<List<Country>> GetCountriesAsync()
        {
            using MySqlCommand command = new();
            command.CommandText = "SELECT `Код` AS 'Id', `Буквенный код` AS 'IdName', `Краткое наименование` AS 'ShortName', `Полное наименование` AS 'FullName' FROM `Страны`";
            return JsonConvert.DeserializeObject<List<Country>>(JsonConvert.SerializeObject(await ProductsDB.Instance.GetDataAsync(command)));
        }
        public async Task InsertCountryAsync(Country country)
        {
            using MySqlCommand command = new();
            command.CommandText = "INSERT INTO `Страны` VALUES(@id, @idName, @shortName, @fullName)";
            command.Parameters.AddWithValue("@id", country.Id);
            command.Parameters.AddWithValue("@idName", country.IdName);
            command.Parameters.AddWithValue("@shortName", country.ShortName);
            command.Parameters.AddWithValue("@fullName", country.FullName);

            await ProductsDB.Instance.SetDataAsync(command);
        }
        public async Task UpdateCountryAsync(uint id, Country country)
        {
            using MySqlCommand command = new();
            command.CommandText = "UPDATE `Страны` SET `Буквенный код` = @idName, `Краткое наименование` = @shortName, `Полное наименование` = @fullName WHERE `Код` = @id";
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@idName", country.IdName);
            command.Parameters.AddWithValue("@shortName", country.ShortName);
            command.Parameters.AddWithValue("@fullName", country.FullName);

            await ProductsDB.Instance.SetDataAsync(command);
        }

        public async Task<List<Supplier>> GetSuppliersAsync()
        {
            using MySqlCommand command = new();
            command.CommandText = "SELECT `Код` AS 'Id', `Наименование` AS 'Name', `Юридический адрес` AS 'Address', `Телефон` AS 'Phone', `Почта` AS 'Mail', `Контактное лицо` AS 'Contact' FROM `Поставщики`";
            return JsonConvert.DeserializeObject<List<Supplier>>(JsonConvert.SerializeObject(await ProductsDB.Instance.GetDataAsync(command)));
        }
        public async Task InsertSupplierAsync(Supplier supplier)
        {
            using MySqlCommand command = new();
            command.CommandText = "INSERT INTO `Поставщики` VALUES(@id, @name, @address, @phone, @mail, @contact)";
            command.Parameters.AddWithValue("@id", supplier.Id);
            command.Parameters.AddWithValue("@name", supplier.Name);
            command.Parameters.AddWithValue("@address", supplier.Address);
            command.Parameters.AddWithValue("@phone", supplier.Phone);
            command.Parameters.AddWithValue("@mail", supplier.Mail);
            command.Parameters.AddWithValue("@contact", supplier.Contact);

            await ProductsDB.Instance.SetDataAsync(command);
        }
        public async Task UpdateSupplierAsync(uint id, Supplier supplier)
        {
            using MySqlCommand command = new();
            command.CommandText = "UPDATE `Поставщики` SET `Наименование` = @name, `Юридический адрес` = @address, `Телефон` = @phone, `Почта` = @mail, `Контактное лицо` = @contact WHERE `Код` = @id";
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@name", supplier.Name);
            command.Parameters.AddWithValue("@address", supplier.Address);
            command.Parameters.AddWithValue("@phone", supplier.Phone);
            command.Parameters.AddWithValue("@mail", supplier.Mail);
            command.Parameters.AddWithValue("@contact", supplier.Contact);

            await ProductsDB.Instance.SetDataAsync(command);
        }

        public async Task<User> GetUserAsync(string hwid, UserLogin userLogin)
        {
            using MySqlCommand command = new();
            command.CommandText = "UPDATE `Пользователи` SET `HWID` = EncryptedString(@hwid) WHERE `Логин` = @login AND `Пароль` = EncryptedString(@pass) AND `HWID` IS NULL; CALL GetUserJSON(@login, @pass, @hwid); ";
            command.Parameters.AddWithValue("@login", userLogin.Login);
            command.Parameters.AddWithValue("@pass", userLogin.Password);
            command.Parameters.AddWithValue("@hwid", hwid);

            return JsonConvert.DeserializeObject<List<User>>((await ProductsDB.Instance.GetDataAsync(command)).Rows[0][0].ToString())?.First();
        }
        public async Task<string> GetLevelAccessUserAsync(string login)
        {
            using MySqlCommand command = new();
            command.CommandText = "SELECT GetLevelAccessUser(@login)";
            command.Parameters.AddWithValue("@login", login);

            return (await ProductsDB.Instance.GetDataAsync(command)).Rows[0][0].ToString();
        }

        public async Task<string> EncryptedString(string line)
        {
            using MySqlCommand command = new();
            command.CommandText = "SELECT EncryptedString(@line)";
            command.Parameters.AddWithValue("@line", line);

            return (await ProductsDB.Instance.GetDataAsync(command)).Rows[0][0].ToString();
        }
    }
}