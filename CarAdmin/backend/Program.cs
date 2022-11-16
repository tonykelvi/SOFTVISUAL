using System;
using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

//dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 6.0
//dotnet add package Microsoft.EntityFrameworkCore.Design --version 6.0
//dotnet ef migrations add InitialCreate
//dotnet ef database update

namespace Trabalho
{
	class Usuario
    {
		public int       Id          {get; set;}
		public string?   Cep         {get; set;}
    	public string?   Cpf         {get; set;}
    	public string?   Complemento {get; set;}
    	public string?   Email       {get; set;}
    	public string?   Logradouro  {get; set;}
    	public DateTime? Nascimento  {get; set;}
    	public string?   Nome        {get; set;}
    	public string?   Numero      {get; set;}
    	public string?   Password    {get; set;}
    	
    	static public string Hash(string password)
		{
			return BitConverter.ToString(SHA256.Create().ComputeHash(System.Text.Encoding.UTF8.GetBytes(password))).Replace("-", String.Empty);
		}
    }
	class Carro
    {
		public int Id { get; set; }
        public string? modelo { get; set; }
		public string? placa { get; set; }
    }

	class Vendedor
	{
		public int id { get; set; }
		public string? nomeFuncionario { get; set; }	
    }

	
	class Database : DbContext
	{
		public Database(DbContextOptions options) : base(options) {}
		public DbSet<Usuario> Usuarios {get; set;} = null!;
		public DbSet<Carro> Carros { get; set; } = null!;
		public DbSet<Vendedor> Vendedores  { get; set; } = null!;
	}
	
	class Program
	{
		static void Main(string[] args)
		{
			
			var builder = WebApplication.CreateBuilder(args);
			builder.Services.AddSqlite<Database>(builder.Configuration.GetConnectionString("Database") ?? "Data Source=Database.db");
			builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
			var app = builder.Build();
			app.UseCors();
						
			////////////////////////////
			/////// CRUD USUARIO //////
			//////////////////////////
			
			app.MapGet("/usuarios", (Database database) =>
			{
				return database.Usuarios.ToList();
			});
			
			app.MapPost("/usuarios", (Database database, Usuario usuario) =>
			{

				if(database.Usuarios.Where(u => u.Email == usuario.Email).Count() > 0)
				{
					return Results.Problem("email indisponivel");
				}

				if(usuario.Password == null || usuario.Password.Length == 0)
				{
					return Results.Problem("password invalido");
				}
				usuario.Password = Usuario.Hash(usuario.Password);
				database.Usuarios.Add(usuario);
				database.SaveChanges();
				return Results.Ok();
			});
			
			app.MapGet("/usuarios/{id}", (Database database, int id) =>
			{
				return database.Usuarios.Find(id);
			});
			
			app.MapPut("/usuarios/{id}", (Database database, Usuario atualizado, int id) =>
			{
				var usuario = database.Usuarios.Find(id);
				if(usuario == null)
				{
					return Results.NotFound();
				}
				if(null != atualizado.Cep)         usuario.Cep         = atualizado.Cep;
				if(null != atualizado.Cpf)         usuario.Cpf         = atualizado.Cpf;
				if(null != atualizado.Complemento) usuario.Complemento = atualizado.Complemento;
				if(null != atualizado.Email)       usuario.Email       = atualizado.Email;
				if(null != atualizado.Logradouro)  usuario.Logradouro  = atualizado.Logradouro;
				if(null != atualizado.Nascimento)  usuario.Nascimento  = atualizado.Nascimento;
				if(null != atualizado.Nome)        usuario.Nome        = atualizado.Nome;
				if(null != atualizado.Numero)      usuario.Numero      = atualizado.Numero;
				database.SaveChanges();
				return Results.Ok();
			});
			
			app.MapDelete("/usuarios/{id}", (Database database, int id) =>
			{
				var usuario = database.Usuarios.Find(id);
				if(usuario == null)
				{
					return Results.NotFound();
				}
				database.Remove(usuario);
				database.SaveChanges();
				return Results.Ok();
			});

			////////////////////////////
			/////// CRUD CARRO ////////
			//////////////////////////

			app.MapGet("/carros", (Database database) => {
				return database.Carros.ToList();

			});
			app.MapPost("/carros", (Database database, Carro carro) =>
			{
				database.Carros.Add(carro);
				database.SaveChanges();
				return Results.Ok();
			});

			app.MapGet("/carros/{id}", (Database database, int id) =>
			{
				return database.Carros.Find(id);
			});

			app.MapPut("/carros/{id}", (Database database, Carro carroAtualizado, int id) =>
			{
				var carro = database.Carros.Find(id);
				if(carro == null)
				{
					return Results.NotFound();
				}				
				if(null != carroAtualizado.modelo)        carro.modelo        = carroAtualizado.modelo;
				if(null != carroAtualizado.placa)      carro.placa      = carroAtualizado.placa;
				database.SaveChanges();
				return Results.Ok();
			});

			app.MapDelete("/carros/{id}", (Database database, int id) =>
			{
				var carro = database.Carros.Find(id);
				if(carro == null)
				{
					return Results.NotFound();
				}
				database.Remove(carro);
				database.SaveChanges();
				return Results.Ok();
			});

			////////////////////////////
			////// CRUD VENDEDOR //////
			//////////////////////////

			app.MapGet("/vendedores", (Database database) => {
				return database.Vendedores.ToList();

			});
			app.MapPost("/vendedores", (Database database, Vendedor vendedor) =>
			{
				database.Vendedores.Add(vendedor);
				database.SaveChanges();
				return Results.Ok();
			});

			app.MapGet("/vendedores/{id}", (Database database, int id) =>
			{
				return database.Vendedores.Find(id);
			});

			app.MapPut("/vendedores/{id}", (Database database, Vendedor vendedorAtualizado, int id) =>
			{
				var vendedor = database.Vendedores.Find(id);
				if(vendedor == null)
				{
					return Results.NotFound();
				}				
				if(null != vendedorAtualizado.nomeFuncionario)        vendedor.nomeFuncionario        = vendedorAtualizado.nomeFuncionario;
				database.SaveChanges();
				return Results.Ok();
			});

			app.MapDelete("/vendedores/{id}", (Database database, int id) =>
			{
				var vendedor = database.Vendedores.Find(id);
				if(vendedor == null)
				{
					return Results.NotFound();
				}
				database.Remove(vendedor);
				database.SaveChanges();
				return Results.Ok();
			});

			
			app.Run("http://localhost:3000");
		}
	}
}
