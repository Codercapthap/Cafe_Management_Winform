drop database if exists QuanLyQuanCafe;
CREATE database QuanLyQuanCafe;
use QuanLyQuanCafe;

CREATE TABLE TableFood(
	id int primary key auto_increment,
    name nvarchar(100) not null,
    status nvarchar(100) not null default N'Empty' -- Trong || co nguoi
);

CREATE TABLE Account(
	id int primary key auto_increment,
    displayName nvarchar(100) not null,
    username nvarchar(100) not null,
    password nvarchar(1000) not null default "0",
    type int not null default 0 -- 1: admin && 0: tablefoodstaff 
);

CREATE TABLE FoodCategory(
	id int primary key auto_increment,
    name nvarchar(100)
);

CREATE TABLE Food(
	id int primary key auto_increment,
    name nvarchar(100) not null,
    idCategory int not null,
    price float not null default 0,
    foreign key (idCategory) references FoodCategory(id)
);

CREATE TABLE Bill(
	id int primary key auto_increment,
    dateCheckIn timestamp not null default current_timestamp(),
    dateCheckOut timestamp,
    idTable int not null,
    status int not null default 0, -- 1: da thanh toan && 0: chua thanh toan
    discount int not null default 0,
    totalPrice float not null default 0,
	foreign key (idTable) references TableFood(id)
);
CREATE TABLE BillInfo(
	id int primary key auto_increment,
    idBill int not null,
    idFood int not null,
    count int not null default 0,
    
    foreign key (idBill) references Bill(id),
    foreign key (idFood) references Food(id)
);

insert into account(username, displayName, password, type) values (N'nahida', N'Nahida', N'1', 1);
insert into account(username, displayName, password, type) values (N'ganyu', N'Ganyu', N'1', 0);

delimiter //
create procedure GetAccountByUserName (IN inputUsername nvarchar(100))
begin
	select * from account where username = inputUsername;
end//
delimiter ;

call GetAccountByUserName("nahida");

delimiter //
create procedure Login(inputUsername nvarchar(100), inputPassword nvarchar(1000)) 
begin
	select * from account where username = inputUsername and password = inputPassword;
end//
delimiter ;

call Login("nahida", "0");

DELIMITER //
CREATE PROCEDURE CreateTableFood()
BEGIN
    DECLARE i INT default 0;
    WHILE i <= 10 DO
        insert into tablefood (name) values (concat('table ', i));
        SET i = i + 1;
    END WHILE;
END//
DELIMITER ;
call CreateTableFood();

DELIMITER //
CREATE PROCEDURE GetTableList()
BEGIN
	SELECT * FROM tablefood;
END//
DELIMITER ;

CALL GetTableList();

-- add category
insert into foodcategory(name) values (N'Seafood');
insert into foodcategory(name) values (N'Agricultural Product');
insert into foodcategory(name) values (N'Forest Product');
insert into foodcategory(name) values (N'Drink');

-- add food
insert into food (name, idCategory, price) values (N'Mực một nắng nướng sa tế', 1, 120000);
insert into food (name, idCategory, price) values (N'Dê hấp xả', 2, 100000);
insert into food (name, idCategory, price) values (N'Nghêu hấp xả', 1, 50000);
insert into food (name, idCategory, price) values (N'Gà nướng hoa ngọt', 2, 130000);
insert into food (name, idCategory, price) values (N'Heo rừng nướng muối ớt', 3, 70000);
insert into food (name, idCategory, price) values (N'7UP', 4, 10000);
insert into food (name, idCategory, price) values (N'Pepsi', 4, 10000);

select * from foodcategory;
select * from food;
select * from bill;
select * from billinfo;

select f.name, bi.count, f.price, bi.count*f.price as totalPrice from billinfo as bi, bill as b, food as f where bi.idBill = b.id and bi.idFood = f.id and b.idTable = 1

delimiter //
create procedure InsertBill(in idTable int)
begin
	insert into bill (dateCheckIn, dateCheckOut, idTable, status, discount) values (current_timestamp(), null, idTable, 0, 0);
end //
delimiter ;

delimiter //
create procedure InsertBillInfo(in idBill int, in idFood int, in count int)
begin
	declare isExistsBillInfo int default 0;
    declare foodCount int default 0;
	declare newCount int default 0;
    select id, b.count into isExistsBillInfo, foodCount from billinfo b where b.idBill = idBill and b.idFood = idFood;
    if (isExistsBillInfo > 0) then
		set newCount =  foodCount + count;
        if (newCount > 0) then
			update BillInfo b set count = foodCount + count where b.idFood = idFood;
        else 
			delete from billinfo b where b.idBill = idBill and b.idFood = idFood;
		end if;
    else
		insert into billinfo (idBill, idFood, count) values (idBill, idFood, count);
    end if;
end//
delimiter ;

delimiter //
create trigger TG_UpdateBillInfo after insert on billinfo for each row
begin
	declare idBill int default new.idbill;
    declare idTable int;
    select b.idTable into idTable from bill b where b.id = idBill and status = 0;
    update tablefood t set status = "Replenish" where t.id = idTable;
end//
delimiter ;

delimiter //
create trigger TG_UpdateBill after update on bill for each row
begin
	declare idBill int default new.id;
    declare idTable int default -1;
    select b.idTable into idTable from bill b where b.id = idBill;
    if (idTable != -1) then
		update tablefood t set status = "Empty" where t.id = idTable;
	end if;
end//
delimiter ;

delimiter //
create procedure Switch_Table(in idTable1 int, in idTable2 int)
begin
	declare idBill1 int;
    declare idBill2 int;
    select b.id into idBill1 from bill b where b.idTable = idTable1 and status = 0;
    select b.id into idBill2 from bill b where b.idTable = idTable2 and status = 0;
    if (idBill1 is not null) then
		update bill set idTable = idTable2 where id = idBill1;
        update tablefood set status = "Replenish" where id = idTable2;
        update tablefood set status = "Empty" where id = idTable1;
	end if;
	if (idBill2 is not null) then
		update bill set idTable = idTable1 where id = idBill2;
        update tablefood set status = "Replenish" where id = idTable1;
        if (idBill1 is null) then
			update tablefood set status = "Empty" where id = idTable2;
		end if;
	end if;
end//
delimiter ;

delimiter //
create procedure GetListBillByDate (in startDate date, in endDate date)
begin
select t.name as "Table name", b.dateCheckIn as "Check In Date", b.dateCheckOut as "Check In Out", b.discount as "Discount", b.totalPrice as "Total Price" from bill b, tablefood t 
where date(b.dateCheckIn) >= startDate and date(b.dateCheckOut) <= endDate and b.status = 1 and b.idTable = t.id;
end //
delimiter ;

delimiter //
create procedure UpdateAccount (in username nvarchar(100), in displayName nvarchar(100), in password nvarchar(1000), in newPassword nvarchar(1000))
begin
	declare isRightPass int default 0;
	select count(*) into isRightPass from account a where  a.username = username and a.password = password;
    if (isRightPass = 1) then
		if (newPassword is null or newPassword = "") then
			update account a set a.displayName = displayName where a.username = username;
        else
			update account a set a.displayName = displayName, a.password = newPassword where a.username = username;
		end if;
	else
		select * from account where id = -1;
    end if;
end//
delimiter ;

delimiter //
create trigger DeleteBillInfo after delete on billinfo for each row
begin
	declare emptyCount int default 0;
	declare idBill int;
    declare idBillInfo int;
    declare idTable int;
    select old.id into idBillInfo;
    select old.idBill into idBill;
    select b.idTable into idTable from bill b where b.id = idBill;
    select count(*) into emptyCount from billinfo bi, bill b where bi.idBill = b.id and b.id = idBill and status = 0;
    if (emptyCount = 0) then
		update tablefood set status = "Empty" where id = idTable;
    end if;
end //
delimiter ;

delimiter //
create procedure GetListBillByDateAndPage (in startDate date, in endDate date, in page int)
begin
declare pageRows int default 10;
declare selectRow int default pageRows * page;
declare exceptRow int default (pageRows * (page - 1));
	with BillShow as (select t.name as "Table name", b.dateCheckIn as "Check In Date", b.dateCheckOut as "Check In Out", b.discount as "Discount", b.totalPrice as "Total Price" from bill b, tablefood t 
	where date(b.dateCheckIn) >= startDate and date(b.dateCheckOut) <= endDate and b.status = 1 and b.idTable = t.id)
	select * from BillShow limit selectRow offset exceptRow;
end //
delimiter ;


delimiter //
create procedure GetNumBillByDate (in startDate date, in endDate date)
begin
	select count(*) from bill b, tablefood t where date(b.dateCheckIn) >= startDate and date(b.dateCheckOut) <= endDate and b.status = 1 and b.idTable = t.id;
end //
delimiter ;


