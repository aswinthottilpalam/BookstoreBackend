create Table AddressType
(
	TypeId int identity(1,1) not null primary key,
	TypeName varchar(255) not null
);

Select * from AddressType;

Insert into AddressType
values('Home'),('Office'),('Other');


-----Table for Address-----
create Table Address
(
	AddressId int identity(1,1) primary key,
	Address varchar(max) not null,
	City varchar(100) not null,
	State varchar(100) not null,
	TypeId int not null 
	FOREIGN KEY (TypeId) REFERENCES AddressType(TypeId),
	UserId INT not null
	FOREIGN KEY (UserId) REFERENCES Users(UserId),
);

select *from Address


------Store Procedure for Add Address----


create proc AddAddress
(
	@Address varchar(max),
	@City varchar(100),
	@State varchar(100),
	@TypeId int,
	@UserId int
)
as
BEGIN
	If exists(select * from AddressType where TypeId=@TypeId)
		begin
			Insert into Address
			values(@Address, @City, @State, @TypeId, @UserId);
		end
	Else
		begin
			select 2
		end
end;

------Procedure for Delete-----

create Proc DeleteAddress
(
	@AddressId int,
	@UserId int
)
as
begin
	Delete Address
	where 
		AddressId=@AddressId and UserId=@UserId;
end;

-----procedure for Update----
 create proc spforUpdateAddress
(
	@Address varchar(255),
    @City varchar(255),
    @State varchar(255),
    @TypeId varchar(255),  
	@UserId varchar(255),
	@AddressId varchar(255)
)
as
begin

update Address set Address =@Address,
				City =@City,
				State  =@State,
				TypeId =@TypeId
				where UserId=@UserId and AddressId=@AddressId;
end;

-----------------------------------------------------------------------------------------
--------------------------- Order book --------------------------------------------------
-----------------------------------------------------------------------------------------

create table Orders
(
	OrderId int identity(1,1) not null primary key,
	TotalPrice int not null,
	BookQuantity int not null,
	OrderDate Date not null,
	UserId INT NOT NULL FOREIGN KEY REFERENCES Users(UserId),
	BookId INT NOT NULL FOREIGN KEY REFERENCES Books(BookId),
	AddressId int not null FOREIGN KEY REFERENCES Address(AddressId)
);

select * from Orders;

Create Proc AddOrders
(
	@BookQuantity int,
	@UserId int,
	@BookId int,
	@AddressId int
)
as
Declare @TotalPrice int
BEGIN
	set @TotalPrice = (select DiscountPrice from Books where BookId = @BookId);
	
			If(Exists (Select * from Books where BookId = @BookId))
				BEGIN
					Begin try
						Begin Transaction
						Insert Into Orders(TotalPrice, BookQuantity, OrderDate, UserId, BookId, AddressId)
						Values(@TotalPrice*@BookQuantity, @BookQuantity, GETDATE(), @UserId, @BookId, @AddressId);
						Update Books set Quantity= Quantity-@BookQuantity where BookId = @BookId;
						Delete from Cart where BookId = @BookId and UserId = @UserId;
						select * from Orders;
						commit Transaction
					End try
					Begin Catch
							rollback;
					End Catch
				END
			
	Else
		Begin
			Select 2;
		End
END;

----------------- Store procedure Getall orders------------------
Create Proc GetAllOrders
(
	@UserId int
)
as
begin
		Select 
		Orders.OrderId, Orders.UserId, Orders.AddressId, Books.BookId,
		Orders.TotalPrice, Orders.BookQuantity, Orders.OrderDate,
		Books.BookName, Books.AuthorName, Books.BookImage
		FROM Books 
		inner join Orders on Orders.BookId = Books.BookId 
		where 
			Orders.UserId = @UserId;
END


---------------------------------------------------------------------------------------
----------------------------- Feedback book section -----------------------------------
---------------------------------------------------------------------------------------

create Table Feedback
(
	FeedbackId INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	Comment varchar(max) not null,
	Rating int not null,
	BookId int not null FOREIGN KEY (BookId) REFERENCES Books(BookId),
	UserId INT NOT NULL FOREIGN KEY (UserId) REFERENCES Users(UserId),
);

drop Table Feedback;

select * from Books;

-------------- Feedback store procedure ------------------
create Proc AddFeedback
(
	@Comment varchar(max),
	@Rating decimal,
	@BookId int,
	@UserId int
)
as
Declare @AverageRating int;
BEGIN
	IF (EXISTS(SELECT * FROM Feedback WHERE BookId = @BookId and UserId=@UserId))
		select 1;
	Else
	Begin
		IF (EXISTS(SELECT * FROM Books WHERE BookId = @BookId))
		Begin  select * from Feedback
			Begin try
				Begin transaction
					Insert into Feedback(Comment, Rating, BookId, UserId) values(@Comment, @Rating, @BookId, @UserId);		
					set @AverageRating = (Select AVG(Rating) from Feedback where BookId = @BookId);
					Update Books set rating = @AverageRating
								 where  BookId = @BookId;
				Commit Transaction
			End Try
			Begin catch
				Rollback transaction
			End catch
		End
		Else
		Begin
			Select 2; 
		End
	End
END

-------------------- getallfeedback storeprocedure -------------------
Create Proc GetAllFeedback
(
	@BookId int
)
as
BEGIN
	Select Feedback.FeedbackId, Feedback.UserId, Feedback.BookId,Feedback.Comment,Feedback.Rating, Users.FullName
	From Users
	Inner Join Feedback
	on Feedback.UserId = Users.UserId
	where
	 BookId = @BookId;
END;