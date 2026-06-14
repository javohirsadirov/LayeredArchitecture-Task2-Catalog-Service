namespace CatalogService.Dtos;

public class LinkedResourceDto<T>
{
    public T Data { get; set; } = default!;
    public List<LinkDto> Links { get; set; } = [];
}

