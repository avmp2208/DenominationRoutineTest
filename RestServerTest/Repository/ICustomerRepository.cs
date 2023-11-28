using RestServerTest.Repository.Entities;

namespace RestServerTest.Repository;

public interface ICustomerRepository
{
    List<Customer> GetCustomers();
    void AddCustomers(List<Customer> newCustomers);
}