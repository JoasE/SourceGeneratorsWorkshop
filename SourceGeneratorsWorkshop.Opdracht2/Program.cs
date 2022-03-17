using SourceGeneratorsWorkshop.Opdracht2;
using AotMapper;

var mapper = new Mapper();

var model = new Model();
Dto result = mapper.Map<Model, Dto>(model);
mapper.Map(model, new Dto());