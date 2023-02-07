INSERT INTO Content
values (2, 'https://imgur.com/a/sagjqxd' )

select * from Content

select* 
from Listing 
join Content on Listing.id_listing = Content.id_listing

ALTER TABLE Content
ADD FOREIGN KEY (id_listing) REFERENCES Listing(id_listing);

