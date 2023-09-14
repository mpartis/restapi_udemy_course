var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// vazoume edw to NewtonsoftJson gia na kanoume patch
builder.Services.AddControllers(option =>
{
    // sthn ousia edw tou lew oti an o client sta headers den exei acceptable to json steile error
    //option.ReturnHttpNotAcceptable=true;
}).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters(); // kai edw tou lew na dexesai kai xml
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
