-- Insertion dans Tiers : Clients et Cuisiniers (dont 2 sont les deux)
INSERT INTO Tiers VALUES
(1, '75001', 'Paris', 'alice@mail.com', '0102030405', 'Dupont', '1 rue de Paris', 'Alice'),
(2, '75002', 'Paris', 'bob@mail.com', '0102030406', 'Durand', '2 rue de Lyon', 'Bob'),
(3, '75003', 'Paris', 'carol@mail.com', '0102030407', 'Moreau', '3 rue de Rome', 'Carol'),
(4, '75004', 'Paris', 'dave@mail.com', '0102030408', 'Bernard', '4 rue de Madrid', 'Dave'),
(5, '75005', 'Paris', 'eve@mail.com', '0102030409', 'Petit', '5 rue de Londres', 'Eve'),
(6, '75006', 'Paris', 'frank@mail.com', '0102030410', 'Noel', '6 rue d’Athènes', 'Frank'),
(7, '75007', 'Paris', 'grace@mail.com', '0102030411', 'Legrand', '7 rue de Rome', 'Grace'),
(8, '75008', 'Paris', 'hugo@mail.com', '0102030412', 'Lemoine', '8 rue d’Amsterdam', 'Hugo'),
(9, '75009', 'Paris', 'irene@mail.com', '0102030413', 'Renard', '9 rue de Londres', 'Irène'),
(10, '75010', 'Paris', 'john@mail.com', '0102030414', 'Martin', '10 rue de Berlin', 'John');
INSERT INTO Tiers VALUES
(11, '75011', 'Paris', 'chef1@mail.com', '0102030415', 'Gault', '11 rue du Chef', 'Michel'),
(12, '75012', 'Paris', 'chef2@mail.com', '0102030416', 'Millau', '12 rue du Chef', 'Paul'),
(13, '75013', 'Paris', 'chef3@mail.com', '0102030417', 'Robuchon', '13 rue du Chef', 'Joël');

-- Insertion Clients (IDs 1 à 10)
INSERT INTO Client VALUES
(1), (2), (3), (4), (5), (6), (7), (8), (9), (10);

-- Insertion Cuisiniers (5 cuisiniers, les IDs 3 et 6 sont aussi clients)
INSERT INTO Cuisinier VALUES
(3), (6), (11), (12), (13);

-- Insertion de 10 plats avec plusieurs par cuisinier
INSERT INTO Plat VALUES
(1, 'Entrée', 'Salade César', '2025-04-01', '2025-04-05', 'Française', 'Végétarien', 'Salade, Parmesan, Croutons', 8.50, 1, 3),
(2, 'Plat', 'Boeuf Bourguignon', '2025-04-01', '2025-04-06', 'Française', 'Omnivore', 'Boeuf, Vin rouge', 15.00, 2, 6),
(3, 'Plat', 'Lasagnes', '2025-04-01', '2025-04-05', 'Italienne', 'Omnivore', 'Pâtes, Viande, Fromage', 12.00, 2, 11),
(4, 'Dessert', 'Tiramisu', '2025-04-02', '2025-04-06', 'Italienne', 'Végétarien', 'Mascarpone, Café', 6.00, 1, 11),
(5, 'Plat', 'Couscous', '2025-04-02', '2025-04-06', 'Marocaine', 'Omnivore', 'Semoule, Légumes, Viande', 13.00, 2, 3),
(6, 'Entrée', 'Soupe Miso', '2025-04-02', '2025-04-04', 'Japonaise', 'Vegan', 'Miso, Tofu', 5.50, 1, 12),
(7, 'Dessert', 'Mochi', '2025-04-01', '2025-04-07', 'Japonaise', 'Végétarien', 'Riz gluant, Sucre', 4.50, 1, 12),
(8, 'Plat', 'Paella', '2025-04-01', '2025-04-06', 'Espagnole', 'Omnivore', 'Riz, Fruits de mer', 14.00, 2, 13),
(9, 'Dessert', 'Crème brûlée', '2025-04-02', '2025-04-05', 'Française', 'Végétarien', 'Crème, Sucre', 6.00, 1, 3),
(10, 'Plat', 'Tajine', '2025-04-01', '2025-04-06', 'Marocaine', 'Omnivore', 'Agneau, Abricot', 14.00, 2, 3),
(11, 'Entrée', 'Bruschetta', '2025-04-03', '2025-04-06', 'Italienne', 'Végétarien', 'Pain, Tomate, Basilic', 5.00, 1, 11),
(12, 'Plat', 'Ramen', '2025-04-03', '2025-04-07', 'Japonaise', 'Omnivore', 'Nouilles, Bouillon, Porc', 13.50, 2, 12),
(13, 'Dessert', 'Baklava', '2025-04-03', '2025-04-08', 'Turque', 'Végétarien', 'Pâte filo, Miel, Noix', 6.50, 1, 13),
(14, 'Plat', 'Pad Thaï', '2025-04-03', '2025-04-07', 'Thaïlandaise', 'Vegan', 'Nouilles, Tofu, Légumes', 11.00, 2, 6);

-- Commandes (au moins un client commande plusieurs plats)
INSERT INTO Commande VALUES
(1, '2025-04-03', '12:30:00', 1, 3),
(2, '2025-03-03', '13:00:00', 2, 6),
(3, '2025-04-03', '13:30:00', 1, 12),
(4, '2025-04-04', '14:00:00', 4, 11),
(5, '2025-04-05', '12:00:00', 5, 11),
(6, '2025-04-05', '19:30:00', 6, 13),
(7, '2025-04-06', '18:45:00', 3, 6);

-- Association Commande <-> Plat
INSERT INTO PlatCommande VALUES
(1, 1), -- client 1 commande Salade César
(1, 9), -- client 1 commande Crème brûlée
(2, 2), -- client 2 commande Boeuf Bourguignon
(3, 6), -- client 1 commande Soupe Miso
(3, 7), -- client 1 commande Mochi
(4, 3), -- client 4 commande Lasagnes
(5, 11), -- client 5 commande Bruschetta
(5, 3),  -- client 5 commande Lasagnes
(6, 13), -- client 6 commande Baklava
(6, 8),  -- client 6 commande Paella
(7, 14), -- client 3 commande Pad Thaï
(7, 2);  -- client 3 commande Boeuf Bourguignon

SELECT Tiers.Prenom, Tiers.Nom, COUNT(Commande.IDCommande) AS NombreCommandes
FROM Commande
JOIN Tiers ON Commande.IDCuisinier = Tiers.IDTiers
GROUP BY Tiers.IDTiers
ORDER BY NombreCommandes DESC;
