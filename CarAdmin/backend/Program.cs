using System;
using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

//RODAR ESSES COMANDOS NA PRIMEIRA VEZ
//dotnet new webapi -minimal -o NomeDoProjeto
//cd NomeDoProjeto
//dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 6.0
//dotnet add package Microsoft.EntityFrameworkCore.Design --version 6.0
//dotnet tool install --global dotnet-ef
//dotnet ef migrations add InitialCreate
//RODAR ESSE COMANDO SEMPRE QUE MEXER NAS CLASSES RELATIVAS A BASE DE DADOS
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
    	
    	//metodo para hash da senha (apenas para demonstracao, nao eh necessario login no trabalho)
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

	
	class Database : DbContext
	{
		public Database(DbContextOptions options) : base(options) {}
		public DbSet<Usuario> Usuarios {get; set;} = null!;
		//adicione dbsets para as demais entidades aqui
		public DbSet<Carro> Carros { get; set; } = null!;
	}
	
	class Program
	{
		static void Main(string[] args)
		{
			///////////////////////////
			//CONFIGURACAO DA APLICACAO
			///////////////////////////
			
			//cria builder da aplicacao
			var builder = WebApplication.CreateBuilder(args);
			
			//adiciona database ao builder
			builder.Services.AddSqlite<Database>(builder.Configuration.GetConnectionString("Database") ?? "Data Source=Database.db");
			
			//adiciona politica permissiva de cross-origin ao builder
			builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
			
			//cria aplicacao usando o builder
			var app = builder.Build();
			
			//ativa a politica de cross-origin
			app.UseCors();
			
			/////////////////
			//CRUD DO USUARIO
			/////////////////
			
			//lista usuarios
			app.MapGet("/usuarios", (Database database) =>
			{
				return database.Usuarios.ToList();
			});
			
			//cria usuario
			app.MapPost("/usuarios", (Database database, Usuario usuario) =>
			{
				//valida email (que precisa ser unico)
				if(database.Usuarios.Where(u => u.Email == usuario.Email).Count() > 0)
				{
					return Results.Problem("email indisponivel");
				}
				//valida password para poder aplicar hash
				if(usuario.Password == null || usuario.Password.Length == 0)
				{
					return Results.Problem("password invalido");
				}
				usuario.Password = Usuario.Hash(usuario.Password);
				database.Usuarios.Add(usuario);
				database.SaveChanges();
				return Results.Ok();
			});
			
			//le usuario
			app.MapGet("/usuarios/{id}", (Database database, int id) =>
			{
				return database.Usuarios.Find(id);
			});
			
			//atualiza usuario (apenas com os dados enviados no json)
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
			
			//deleta usuario
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

			/////////////////////////
			// CADASTRO DE CARROS //
			////////////////////////

			//LISTAR CARROS
			app.MapGet("/carros", (Database database) => {
				return database.Carros.ToList();

			});
			app.MapPost("/carros", (Database database, Carro carro) =>
			{
				database.Carros.Add(carro);
				database.SaveChanges();
				return Results.Ok();
			});

			//LER CARROS
			app.MapGet("/carros/{id}", (Database database, int id) =>
			{
				return database.Carros.Find(id);
			});

			//ATUALIZAR CARROS
			app.MapPost("/carros{id}", (Database database, Carro carroAtualizado, int id) =>
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

			//REMOVER CARROS
			app.MapPost("/carros{id}", (Database database, int id) =>
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
			// EXECUCAO DA APLICACAO //
			//////////////////////////
			
			//roda aplicacao na porta 3000 (arbitraria)
			app.Run("http://localhost:3000");
		}
	}
}
