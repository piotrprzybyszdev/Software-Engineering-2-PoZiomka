using Dapper;
using Microsoft.Data.SqlClient;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.Student;
using PoZiomkaDomain.Student.Dtos;
using PoZiomkaInfrastructure.Constants;
using PoZiomkaInfrastructure.Exceptions;
using System.Data;
using PoZiomkaDomain.Application;

namespace PoZiomkaInfrastructure.Repositories;

public class ApplicationRepository(IDbConnection connection) : IApplicationRepository
{
    
}
