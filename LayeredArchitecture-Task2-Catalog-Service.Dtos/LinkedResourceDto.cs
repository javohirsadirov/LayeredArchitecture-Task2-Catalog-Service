namespace LayeredArchitecture_Task2_Catalog_Service.Dtos;

public class LinkedResourceDto<T>
{
    public T Data { get; set; } = default!;
    public List<LinkDto> Links { get; set; } = [];
}
