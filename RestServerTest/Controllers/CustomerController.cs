using Microsoft.AspNetCore.Mvc;
using RestServerTest.Repository;
using RestServerTest.Repository.Entities;

[ApiController]
[Route("[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerController(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
    }

    [HttpPost("customers")]
    public IActionResult PostCustomers([FromBody] List<Customer> newCustomers)
    {
        try
        {
            _customerRepository.AddCustomers(newCustomers);
            return Ok();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("customers")]
    public IActionResult GetCustomers()
    {
        var customers = _customerRepository.GetCustomers();
        return Ok(customers);
    }
}