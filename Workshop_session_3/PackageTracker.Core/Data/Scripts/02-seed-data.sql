-- PackageStatuses (w języku polskim)
SET IDENTITY_INSERT PackageStatuses ON;
INSERT INTO PackageStatuses (Id, Name) VALUES
(1, N'Przyjęta do nadania'),
(2, N'W drodze'),
(3, N'Dostarczona'),
(4, N'Oczekuje na odbiór'),
(5, N'Zwrot do nadawcy');
SET IDENTITY_INSERT PackageStatuses OFF;

-- CarrierServices
SET IDENTITY_INSERT CarrierServices ON;
INSERT INTO CarrierServices (Id, Name, Description, Price) VALUES
(1, N'Standard', N'Dostawa w 3–5 dni roboczych', 12.99),
(2, N'Express', N'Dostawa w 1–2 dni robocze', 24.99),
(3, N'Międzynarodowa', N'Dostawa poza granice kraju', 49.99);
SET IDENTITY_INSERT CarrierServices OFF;

-- Carriers
SET IDENTITY_INSERT Carriers ON;
INSERT INTO Carriers (Id, Name, Email, PhoneNumber, IsActive) VALUES
(1, N'Kurier24', 'kontakt@kurier24.pl', '123456789', 1),
(2, N'GlobEx', 'info@globex.com', '987654321', 1);
SET IDENTITY_INSERT Carriers OFF;

-- CarrierSupportedServices (mapowanie wielu do wielu)
INSERT INTO CarrierSupportedServices (CarrierId, ServiceId) VALUES
(1, 1), -- Kurier24 -> Standard
(1, 2), -- Kurier24 -> Express
(2, 3), -- GlobEx -> Międzynarodowa
(2, 1); -- GlobEx -> Standard
