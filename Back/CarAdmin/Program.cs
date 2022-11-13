using System;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
// RODAR PROJETO - SE ATENTAR A PASTAR QUE VAI RODAR "CarAdmin"
// dotnet ef database update
// dotnet run

namespace Trabalho
{
	class Cliente
    {
    	public int id { get; set; }
		public string? nome { get; set; }
		public string? contato { get; set; }
    	public string? email { get; set; }        
    }
	class Carro
    {
		public int id { get; set; }
        public string? modelo { get; set; }
		public string? placa { get; set; }
    }
	class Vendedor
	{
		public int id { get; set; }
		public string? nomeFuncionario { get; set; }	
    }
	class Emprestimo
	{
        public int id { get; set; }
		public int idCliente { get; set; }
		public int idCarro { get; set; }
		public int idVendedor { get; set; }
		public string? dataempr { get; set; }
        public string? datadev { get; set; }  
    }
		
	class BaseRentCar : DbContext
	{
		public BaseRentCar(DbContextOptions options) : base(options)
		{
		}
		
		public DbSet<Cliente> Clientes { get; set; } = null!;
        public DbSet<Carro> Carros { get; set; } = null!;
        public DbSet<Vendedor> Vendedores { get; set; } = null!;
        public DbSet<Emprestimo> Emprestimos { get; set; } = null!;
	}
	
	class Program
	{
		static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			
			var connectionString = builder.Configuration.GetConnectionString("BaseRentCar") ?? "Data Source=BaseRentCar.db";
			builder.Services.AddSqlite<BaseRentCar>(connectionString);
			
			var app = builder.Build();
			
            //CADASTRAR
			app.MapPost("/cadastrar/cliente", (BaseRentCar BaseRentCar, Cliente cliente) =>
			{
				BaseRentCar.Clientes.Add(cliente);
				BaseRentCar.SaveChanges();
				return "Novo cliente adicionado com sucesso";
			});

			app.MapPost("/cadastrar/carro", (BaseRentCar BaseRentCar, Carro carro) =>
			{
				BaseRentCar.Carros.Add(carro);
				BaseRentCar.SaveChanges();
				return "Novo Carro adicionado com sucesso";
			});
			app.MapPost("/cadastrar/vendedor", (BaseRentCar BaseRentCar, Vendedor vendedor) =>
			{
				BaseRentCar.Vendedores.Add(vendedor);
				BaseRentCar.SaveChanges();
				return "Novo Vendedor adicionado com sucesso";
			});


			//LISTAR
			app.MapGet("/listar/cliente", (BaseRentCar BaseRentCar) => {
				return BaseRentCar.Clientes.ToList();
			});
			
			app.MapGet("/listar/cliente/{id}", (BaseRentCar BaseRentCar, int id) => {
				return BaseRentCar.Clientes.Find(id);
			});
			app.MapGet("/listar/carros", (BaseRentCar BaseRentCar) => {
				return BaseRentCar.Carros.ToList();
			});
			
			app.MapGet("/listar/carros/{id}", (BaseRentCar BaseRentCar, int id) => {
				return BaseRentCar.Carros.Find(id);
			});
			app.MapGet("/listar/vendedor", (BaseRentCar BaseRentCar) => {
				return BaseRentCar.Vendedores.ToList();
			});
			
			app.MapGet("/listar/vendedor/{id}", (BaseRentCar BaseRentCar, int id) => {
				return BaseRentCar.Vendedores.Find(id);
			});


			//ATUALIZAR 
			app.MapPost("/atualizar/cliente/{id}", (BaseRentCar BaseRentCar, Cliente clienteAtualizado, int id) =>
			{
				var cliente = BaseRentCar.Clientes.Find(id);
				cliente.nome = clienteAtualizado.nome;
				cliente.email = clienteAtualizado.email;
                cliente.contato = clienteAtualizado.contato;
				BaseRentCar.SaveChanges();
				return "Cliente atualizado com sucesso";
			});
			app.MapPost("/atualizar/carro/{id}", (BaseRentCar BaseRentCar, Carro carroAtualizado, int id) =>
			{
				var carro = BaseRentCar.Carros.Find(id);
				carro.modelo = carroAtualizado.modelo;
				carro.placa = carroAtualizado.placa;
				BaseRentCar.SaveChanges();
				return "Carro atualizado com sucesso";
			});
			app.MapPost("/atualizar/vendedor/{id}", (BaseRentCar BaseRentCar, Vendedor vendedorAtualizado, int id) =>
			{
				var vendedor = BaseRentCar.Vendedores.Find(id);
				vendedor.nomeFuncionario = vendedorAtualizado.nomeFuncionario;
				BaseRentCar.SaveChanges();
				return "Vendedor atualizado com sucesso";
			});


			//DELETAR
			app.MapPost("/deletar/cliente/{id}", (BaseRentCar BaseRentCar, int id) =>
			{
				var cliente = BaseRentCar.Clientes.Find(id);
				BaseRentCar.Remove(cliente);
				BaseRentCar.SaveChanges();
				return "Cliente excluido com sucesso";
			});
			app.MapPost("/deletar/carro{id}", (BaseRentCar BaseRentCar, int id) =>
			{
				var carro = BaseRentCar.Carros.Find(id);
				BaseRentCar.Remove(carro);
				BaseRentCar.SaveChanges();
				return "Carro excluido com sucesso";
			});
			app.MapPost("/deletar/vendedor{id}", (BaseRentCar BaseRentCar, int id) =>
			{
				var vendedor = BaseRentCar.Vendedores.Find(id);
				BaseRentCar.Remove(vendedor);
				BaseRentCar.SaveChanges();
				return "Vendedor excluido com sucesso";
			});						
			app.Run();
		}
	}
}