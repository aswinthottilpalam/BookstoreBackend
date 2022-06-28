create database BookStore;

Create table Users
( UserId int identity(1,1) Primary key,
FullName Varchar(225) not null,
Email Varchar(225) not null unique,
Password varchar(225) not null,
MobileNumber bigint not null
)

select *from Users

------------- Resistration store Procedure-----------
Create procedure SPUserRegister
(
@FullName varchar(255),
@Email varchar(255),
@Password Varchar(255),
@MobileNumber Bigint
)
as
Begin
		insert Users
		values (@FullName, @Email, @Password, @MobileNumber) 
end

-------- Login Store Procedure-----------
create procedure spUserLogin
(
@Email varchar(255),
@Password varchar(255)
)
as
begin
select * from Users
where Email = @Email and Password = @Password
End;

------------- forgot password Store procedure ----------
create procedure spUserForgotPassword
(
@Email varchar(Max)
)
as
begin
Update Users
set Password = 'Null'
where Email = @Email;
select * from Users where Email = @Email;
End;

------------- Reset Password Store procedure-----------
create procedure spUserResetPassword
(
@Email varchar(250),
@Password varchar(250)
)
AS
BEGIN
UPDATE Users 
SET 
Password = @Password 
WHERE Email = @Email;
End;

----------------------------------
-------- BOOK DETAILS ------------
----------------------------------

---------Create Book Table--------
create table Books(
BookId int identity (1,1)primary key,
BookName varchar(255),
AuthorName varchar(255),
Rating varchar(255),
TotalReview int,
OriginalPrice decimal,
DiscountPrice decimal,
BookDetails varchar(255),
BookImage varchar(255),
Quantity int
);

select *from Books

----------Add book store Procedure ---------
create procedure spAddBook
(
@BookName varchar(255),
@authorName varchar(255),
@rating varchar(100),
@totalReview int,
@originalPrice Decimal,
@discountPrice Decimal,
@BookDetails varchar(255),
@bookImage varchar(255),
@Quantity int
)
as
BEGIN
Insert into Books(BookName, authorName, rating, totalreview, originalPrice, 
discountPrice, BookDetails, bookImage, Quantity)
values (@BookName, @authorName, @rating, @totalReview ,@originalPrice, @discountPrice,
@BookDetails, @bookImage,@Quantity);
End;


DROP PROCEDURE AddBook;
DROP TABLE Books;

------------- Store Procedure for Update Book Details-----------------
create procedure spUpdateBook
(
@BookId int,
@BookName varchar(255),
@authorName varchar(255),
@rating varchar(100),
@totalReview int,
@originalPrice Decimal,
@discountPrice Decimal,
@BookDetails varchar(255),
@bookImage varchar(255),
@Quantity int
)
as
BEGIN
Update Books set BookName = @BookName, 
authorName = @authorName,
rating = @rating,
totalReview =@totalReview,
originalPrice= @originalPrice,
discountPrice = @discountPrice,
BookDetails = @BookDetails,
bookImage =@bookImage,
Quantity = @Quantity
where BookId = @BookId;
End;


------------- Delete store Procedure
create procedure spDeleteBook
(
@BookId int
)
as
BEGIN
Delete Books 
where BookId = @BookId;
End;

------------- Get book by book Id store procedure ---------
create procedure spGetBookByBookId
(
@BookId int
)
as
BEGIN
select * from Books
where BookId = @BookId;
End;

------------ Getall Books
create procedure spGetAllBooks
as
BEGIN
	select * from Books;
End;

------------------------------------------
-------------Admin Table-----------------
----------------------------------------------
create Table AdminTable
(
AdminId int primary key identity(1,1),
FullName varchar(255),
Email varchar(255),
Password varchar(255),
PhoneNumber Bigint
);

Insert into AdminTable values('Admin', 'aswintesterdemo@gmail.com', 'Tester123', '9998887770');
select * from AdminTable

create procedure AdminLogin
(
@Email varchar(255),
@Password varchar(255)
)
as
BEGIN
If(Exists(select * from AdminTable where Email = @Email and Password = @Password))
Begin
select AdminId, FullName,Password, Email, PhoneNumber from AdminTable;
end
Else
Begin
select 2;
End
END;