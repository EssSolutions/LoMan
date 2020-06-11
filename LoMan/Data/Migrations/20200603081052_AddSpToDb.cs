using Microsoft.EntityFrameworkCore.Migrations;

namespace LoMan.Data.Migrations
{
    public partial class AddSpToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sp = @"
/****** Object:  StoredProcedure [dbo].[Set_Dashboard]    Script Date: 03-06-2020 13:45:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Set_Dashboard]
AS
DECLARE
        @Pinterest float,
        @Pprinciple float,
        @Cinterest float,
        @Cprinciple float,
        @Tprinciple float,
        @Tinterest float,        
        @PCount int,
        @TCount int,
        @MCount int,
        @Check int
BEGIN
        SELECT @Pprinciple = Principle , @Pinterest = Interest 
        FROM Analytics
        WHERE Month = (MONTH(GETDATE())-1) AND Year = YEAR(GETDATE());

        SELECT @Cprinciple = SUM(Principle) , @Cinterest = SUM(Interest)
        FROM Recoveries
        WHERE MONTH(Date) = MONTH(GETDATE());

        SELECT @Tprinciple = SUM(Principle) , @Tinterest = SUM(Interest)
        FROM Recoveries;        

        SELECT @TCount = COUNT(*) FROM Loans;

        SELECT @PCount = COUNT(*) FROM Loans WHERE MONTH(Idate) = (MONTH(GETDATE())-1) AND YEAR(Idate) = (YEAR(GETDATE()));

		SELECT @MCount = COUNT(*) FROM Loans WHERE MONTH(Idate) = (MONTH(GETDATE())) AND YEAR(Idate) = (YEAR(GETDATE()));

        SET @Check = 0;      

        SELECT @Check = COUNT(*) 
        FROM Analytics
        WHERE Month = (MONTH(GETDATE())) AND Year = (YEAR(GETDATE()));
 
        IF @Check = 0
        	INSERT into Analytics values(MONTH(GETDATE()),YEAR(GETDATE()),@Cprinciple,@Cinterest,0,0);
        ELSE
        	UPDATE Analytics set principle = @Cprinciple,interest = @Cinterest , Ppercent = 0 , Ipercent = 0 where month = (month(GETDATE())) AND year = YEAR(GETDATE());

        UPDATE Dashboard set TotalLoans = @TCount , Tprinciple = @Tprinciple, Tinterest = @Tinterest , MonthlyLoans = @MCount , Mprinciple = @Cprinciple, Minterest = @Cinterest , PreviousLoans = @PCount , Printerest = @Pinterest , Prprinciple = @Pprinciple where Id =1;
END";


            migrationBuilder.Sql(sp);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
