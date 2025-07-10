using RPBDISlab2;

using (var context = new ToplivoContext())
{
    bool continueRunning = true;
    while (continueRunning)
    {
        //Console.Clear();

        Console.WriteLine(@"
    1 - Вывести все виды топлива
    2 - Вывести топливо с плотностью выше 1.5
    3 - Общая прибыль по каждому виду топлива
    4 - Вывести список топлива с датой каждой операции
    5 - Вывести отфильтрованный список операций новее 2010 года
    6 - Добавить новое топливо
    7 - Добавить новую операцию
    8 - Удалить топливо по ID
    9 - Удалить операцию по ID
    10 - Увеличить вес резервуаров с ID более 490 на 1 единицу
    0 - Выход
    ");
        if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 0 && choice < 11)
        {
            switch (choice)
            {
                case 1:
                    {
                        DisplayAllFuels(context);
                        Pause();
                        break;
                    }
                case 2:
                    {
                        DisplayFuelsWithDensityAbove(context, 1.5);
                        Pause();
                        break;
                    }
                case 3:
                    {
                        TotalIncExpByFuel(context);
                        Pause();
                        break;
                    }
                case 4:
                    {
                        FuelsByOperationDates(context);
                        Pause();
                        break;
                    }
                case 5:
                    {
                        FuelsAndOperationsNewerThan(context, new DateOnly(2010, 1, 1));
                        Pause();
                        break;
                    }
                case 6:
                    {
                        var newFuel = new Fuel();
                        newFuel.FuelType = "Diesel";
                        newFuel.FuelDensity = 1111;
                        AddNewFuelToDB(context, newFuel);
                        DisplayAllFuels(context);
                        Pause();
                        break;
                    }
                case 7:
                    {
                        var newOperation = new RPBDISlab2.Operation { FuelId = 1, TankId = 1, IncExp = 0, Date = new DateOnly(2025, 7, 5) };
                        AddNewOperationToDB(context, newOperation);
                        foreach (var item in context.Operations.ToList().TakeLast(10))
                        {
                            Console.WriteLine($"ID: {item.OperationId}, Fuel ID: {item.FuelId}," +
                                $" Tank ID: {item.TankId}, Inc/Exp: {item.IncExp}, Date: {item.Date}");
                        }
                        Pause();
                        break;
                    }
                case 8:
                    {
                        Console.WriteLine("Введите ID топлива, которое хотите удалить: ");
                        int.TryParse(Console.ReadLine(), out int id);
                        DeleteFuelById(context, id);
                        DisplayAllFuels(context);
                        Pause();
                        break;
                    }
                case 9:
                    {
                        Console.WriteLine("Введите ID операции, которую хотите удалить: ");
                        int.TryParse(Console.ReadLine(), out int id);
                        DeleteOperationById(context, id);
                        DisplayAllOperations(context);
                        Pause();
                        break;
                    }
                case 10:
                    {
                        var tanksToUpdate = context.Tanks.Where(t => t.TankId > 490).ToList();
                        foreach (var tank in tanksToUpdate)
                        {
                            tank.TankWeight = tank.TankWeight + 1;
                        }
                        context.SaveChanges();
                        DisplayAllTanks(context);
                        Pause();
                        break;
                    }
                case 0:
                    {
                        continueRunning = false;
                        break;
                    }
            }
        }
        else
        {
            Console.WriteLine("Ошибка: введите корректное значение!");
        }
    }

        
}

void DisplayAllFuels(ToplivoContext context)
    {
        var fuels = context.Fuels.ToList();
        foreach (var fuel in fuels)
        {
            Console.WriteLine($"ID: {fuel.FuelId}, Тип: {fuel.FuelType}, Плотность: {fuel.FuelDensity}");
        }
    }

void DisplayAllTanks(ToplivoContext context)
{
    var tanks = context.Tanks.ToList();
    foreach (var tank in tanks)
    {
        Console.WriteLine($"ID: {tank.TankId}, Tank type: {tank.TankType}, Tank Volume: " +
            $"{tank.TankVolume}, Tank Weight: {tank.TankWeight}, Tank Material: " +
            $"{tank.TankMaterial}, Tank Picture: {tank.TankPicture}");
    }
}

void DisplayAllOperations(ToplivoContext context)
{
    var operations = context.Operations.ToList();
    foreach (var operation in operations)
    {
        Console.WriteLine($"ID: {operation.OperationId}, Fuel ID: {operation.FuelId}," +
            $" Tank ID: {operation.TankId}, Inc/Exp: {operation.IncExp}, Date: {operation.Date}");
    }
}

void DisplayFuelsWithDensityAbove(ToplivoContext context, double minDensity)
{
    Console.WriteLine($"\n=== Топливо с плотностью > {minDensity} ===");
    var fuels = context.Fuels.ToList();
    var filtredFuels = from f in fuels where f.FuelDensity > minDensity select f;
    foreach (var fuel in filtredFuels)
    {
        Console.WriteLine($"ID: {fuel.FuelId}, Fuel type: {fuel.FuelType}, Fuel density: {fuel.FuelDensity}");
    }
}

void TotalIncExpByFuel (ToplivoContext context)
{
    var operations = context.Operations.ToList();
    var groupedOperations = operations
        .GroupBy(x => x.FuelId)
        .Select(group => new
        {
            FuelId = group.Key,
            TotalIncExp = group.Sum(x => x.IncExp)
        })
        .OrderBy(x => x.FuelId)
        .ToList();

    foreach (var operation in groupedOperations)
    {
        Console.WriteLine($"Fuel ID: {operation.FuelId}, Total inc/exp: {operation.TotalIncExp}");
    }
}

void FuelsByOperationDates (ToplivoContext context)
{
    var fuelsAndOperationDates = context.Fuels
        .Join(
        context.Operations,
        fuel => fuel.FuelId,
        operation => operation.FuelId,
        (fuel, operation) => new { fuel.FuelType, operation.Date })
        .ToList();

    foreach (var item in fuelsAndOperationDates)
    {
        Console.WriteLine(item);
    }
}

void FuelsAndOperationsNewerThan (ToplivoContext context, DateOnly date)
{
    var filteredFuelsAndOperations = from fuel in context.Fuels
                                        join operation in context.Operations
                                        on fuel.FuelId equals operation.FuelId
                                        orderby operation.Date
                                        where operation.Date > date
                                        select new { fuel.FuelId, operation.Date };

    foreach (var item in filteredFuelsAndOperations)
    {
        Console.WriteLine(item);
    }
}

void AddNewFuelToDB (ToplivoContext context, Fuel newFuel)
{
    context.Fuels.Add(newFuel);
    context.SaveChanges();
}

void AddNewOperationToDB (ToplivoContext context, RPBDISlab2.Operation newOperation)
{
    context.Operations.Add(newOperation);
    context.SaveChanges();
}

void DeleteFuelById (ToplivoContext context, int id)
{
    var fuelToDelete = context.Fuels.Single(f => f.FuelId == id);
    context.Fuels.Remove(fuelToDelete);
    context.SaveChanges();
}

void DeleteOperationById (ToplivoContext context, int id)
{
    var operationToDelete = context.Operations.Single(o => o.OperationId == id);
    context.Operations.Remove(operationToDelete);
    context.SaveChanges();
}

void Pause()
{
    Console.WriteLine("Нажмите любую клавишу для продолжения...");
    Console.ReadKey();
}
