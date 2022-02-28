﻿using Dapper;
using Data.Interfaces.DataConnector;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly IDbConnector _dbConnector;

        public UsuarioRepository(IDbConnector dbConnector)
        {
            _dbConnector = dbConnector;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            string sql = " Delete FROM [dbo].[Usuario] Where Id = @Id";
            var delete = await _dbConnector.dbConnection.ExecuteAsync(sql, new { Id = id }, _dbConnector.dbTransaction);
            return Convert.ToBoolean(delete);
        }

        public async Task<Usuario> GetAsync(int id)
        {
            string sql = "SELECT Id,Nome,Email,DataCriacao,DataAtualizacao FROM [dbo].[Usuario] Where Id = @Id";
            var usuario = await _dbConnector.dbConnection.QueryFirstOrDefaultAsync<Usuario>(sql, new {Id = id}, _dbConnector.dbTransaction);
            return usuario;
        }

        public async Task<IEnumerable<Usuario>> GetAsync()
        {
            string sql = "SELECT Id,Nome,Email,DataCriacao,DataAtualizacao FROM [dbo].[Usuario]";
            var usuarios = await _dbConnector.dbConnection.QueryAsync<Usuario>(sql, _dbConnector.dbTransaction);
            return usuarios.ToList();
        }

        public async Task<Usuario> PostAsync(Usuario data)
        {

            string sql = @"INSERT INTO [dbo].[Usuario]
                                 ([Nome]
                                 ,[Email]
                                 ,[DataCriacao]) OUTPUT Inserted.Id
                           VALUES
                                 (@Nome
                                 ,@Email
                                 ,@DataCriacao)";

            data.SetId(await _dbConnector.dbConnection.QuerySingleAsync<int>(sql, new
            {
                Nome = data.Nome,
                Email = data.Email,
                DataCriacao = DateTime.Now,
            }, _dbConnector.dbTransaction));

            return data;
        }

        public async Task<Usuario> PutAsync(int id, Usuario usuario)
        {
            string sql = @"Update [dbo].[Usuario] Set
                                 Nome = @Nome
                                 ,Email = @Email
                                 ,DataAtualizacao = @DataAtualizacao Where Id = @Id";

            await _dbConnector.dbConnection.ExecuteAsync(sql, new
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                DataAtualizacao = DateTime.Now,
            }, _dbConnector.dbTransaction);
            return usuario;
        }
        

    }
}
