ALTER TABLE Packages 
ADD ServiceType INT NOT NULL
FOREIGN KEY (ServiceType) REFERENCES CarrierServices(Id)