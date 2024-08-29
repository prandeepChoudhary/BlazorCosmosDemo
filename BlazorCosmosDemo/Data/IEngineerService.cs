using System;
namespace BlazorCosmosDemo.Data
{
	public interface IEngineerService
	{
        Task DeleteEngineer(string? id, string? partitionKey);
        Task UpsertEngineer(Engineer engineer);
        Task<List<Engineer>> GetEngineerDetails();
        Task<Engineer> GetEngineerById(string? id, string? partitionKey);
    }
}

