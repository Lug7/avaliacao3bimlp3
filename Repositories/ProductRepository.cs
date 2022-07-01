using Avaliacao3BimLp3.Database;
using Avaliacao3BimLp3.Models;
using Microsoft.Data.Sqlite;
using Dapper;

namespace Avaliacao3BimLp3.Repositories;

class ProductRepository
{
    private DatabaseConfig  databaseConfig;

    public ProductRepository(DatabaseConfig databaseConfig)
    {
        this.databaseConfig=databaseConfig;
    }

    public IEnumerable<Product> GetAll()
    {
        using var connection = new SqliteConnection(databaseConfig.ConnectionString);
        connection.Open();

        var products = connection.Query<Product>("SELECT * FROM Products");
           
        return products;
    }
    public Product Save(Product product)
    {
        using var connection = new SqliteConnection(databaseConfig.ConnectionString);
        connection.Open();

        connection.Execute("INSERT INTO Products VALUES(@Id, @Name, @Price, @Active)", product);
        
        return product;
    }
    public void Delete(int id)
    {
        using var connection = new SqliteConnection(databaseConfig.ConnectionString);
        connection.Open();

        connection.Execute("DELETE FROM Products WHERE Id= @Id", new{Id= id});
    }

     public void Enable(int id)
    {
        using var connection = new SqliteConnection(databaseConfig.ConnectionString);
        connection.Open();

        connection.Execute("UPDATE products SET active = True WHERE Id = @id",new {id = id});

    }

     public void Disable(int id)
     {
         using var connection = new SqliteConnection(databaseConfig.ConnectionString);
        connection.Open();

         connection.Execute("UPDATE products SET active = False WHERE Id = @id",new {id = id});

     }

     public bool existsById (int id)
    {
        using var connection = new SqliteConnection(databaseConfig.ConnectionString);
        connection.Open();

        var result = connection.ExecuteScalar<bool>("SELECT count(id) FROM Products WHERE id = @Id", new {Id= id});

        return result;
    }
    public List<Product> GetAllWithPriceBetween(double initialPrice, double endPrice)
    {
        using var connection = new SqliteConnection(databaseConfig.ConnectionString);
        connection.Open();
        var products = connection.Query<Product>("SELECT * FROM products WHERE price > @initialPrice AND price < @endPrice", new {initialPrice = initialPrice, endPrice = endPrice}).ToList();
        return products;
    }
    public List<Product> GetAllWithPriceHigherThan(double price)
    {
        using var connection = new SqliteConnection(databaseConfig.ConnectionString);
        connection.Open();
        var products = connection.Query<Product>("SELECT * FROM products WHERE price > @price", new {price = price}).ToList();
        return products;
    }
    public List<Product> GetAllWithPriceLowerThan(double price)
    {
        using var connection = new SqliteConnection(databaseConfig.ConnectionString);
        connection.Open();
        var products = connection.Query<Product>("SELECT * FROM products WHERE price < @price", new {price = price}).ToList();
        return products;
    }

    public double GetAveragePrice(){
        using var connection = new SqliteConnection(databaseConfig.ConnectionString);
        connection.Open();
        double avarage = connection.ExecuteScalar<double>("SELECT AVG(price) FROM products");
        return avarage;
    }


}