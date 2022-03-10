using AotMapper;
using SourceGeneratorsWorkshop.Opdracht3;

var mapper = new Mapper();

var model = new Model
{
    Id = 1,
    Name = "Name"
};
var result = mapper.Map<Model, Dto>(model);

if (result.Id != model.Id || result.Name != model.Name)
{
    throw new Exception();
}
