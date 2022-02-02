declare @test nvarchar(32)
set @test = 'aaa'
select top 10 * from tablea
set @test = 'bbb'
select @test

