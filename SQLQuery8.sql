-------------------------------------
--------------- CART ----------------
-------------------------------------

Create Table Cart
(
CartId int identity(1,1) primary key,
BookQuantity int default 1,
UserId int not null foreign key (UserId) references Users(UserId),
BookId int not null Foreign key (BookId) references Books(BookId)
)

select  *  From Cart

----- AddCart store procedure--------
Create procedure spAddcart
( @BookQuantity int,
@UserId int,
@BookId int
)
As
Begin
	insert into cart(BookQuantity,UserId, BookId)
	values ( @BookQuantity,@UserId, @BookId);
End

------------Remove from cart store procedure-------------

Create procedure spRemoveFromCart
(
@CartId int
)
As
Begin
	delete from Cart where CartId = @CartId;
end

--------------getcart by userId Store procedure--------------
create procedure spGetCartByUserId
(
	@UserId int
)
as
begin
	select CartId,BookQuantity,UserId,c.BookId,BookName,AuthorName,
	DiscountPrice,OriginalPrice,BookImage from Cart c join Books b on c.BookId=b.BookId 
	where UserId=@UserId;
end;

------Update Store Procedure-----

create procedure spUpdateCart
(
	@BookQuantity int,
	@BookId int,
	@UserId int,
	@CartId int
)
as
begin
update Cart set BookId=@BookId,
				UserId=@UserId,
				BookQuantity=@BookQuantity
				where CartId=@CartId;
end;


-----------------------------------------
--------------- WishList ----------------
-----------------------------------------

----------Create table for wishlist------
create table WishList
(
	WishListId int identity(1,1) not null primary key,
	UserId int foreign key references Users(UserId) on delete no action,
	BookId int foreign key references Books(BookId) on delete no action
);

------- AddToWishlist storeProcedure-----------

create procedure spAddWishList
(
@UserId int,
@BookId int
)
as
begin 
       insert into WishList
	   values (@UserId,@BookId);
end;

-------------Delete WishList Store procedure-------
create procedure spDeleteWishList
(
@WishListId int,
@UserId int

)
as
begin
delete WishList where WishListId = @WishListId;
end;

--------------- Get wishlist by userId store procedure----------
create procedure spGetWishListByUserId
(
	@UserId int
)
as
begin
	select WishListId,UserId,c.BookId,BookName,AuthorName,
	DiscountPrice,OriginalPrice,BookImage from WishList c join Books b on c.BookId=b.BookId 
	where UserId=@UserId;
end;


----------------------------
select * from Users
-----------------------------


