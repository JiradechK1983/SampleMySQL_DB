-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Jul 19, 2023 at 09:38 AM
-- Server version: 10.4.28-MariaDB
-- PHP Version: 8.2.4

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `samplemysql`
--

CREATE DATABASE samplemysql;

USE samplemysql;

-- --------------------------------------------------------

--
-- Table structure for table `customer_tbl`
--

CREATE TABLE `customer_tbl` (
  `cid` char(6) NOT NULL,
  `cname` varchar(40) DEFAULT NULL,
  `address` varchar(30) DEFAULT NULL,
  `telephone` varchar(10) DEFAULT NULL,
  `credit_lim` int(11) DEFAULT NULL,
  `curr_bal` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `employee_tbl`
--

CREATE TABLE `employee_tbl` (
  `eid` char(6) NOT NULL,
  `ename` varchar(40) DEFAULT NULL,
  `salary` decimal(8,2) DEFAULT NULL,
  `address` varchar(30) DEFAULT NULL,
  `telephone` varchar(10) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- Dumping data for table `employee_tbl`
--

INSERT INTO `employee_tbl` (`eid`, `ename`, `salary`, `address`, `telephone`) VALUES
('1', 'Jiradech', 200.00, 'dkd', '99');

-- --------------------------------------------------------

--
-- Table structure for table `order_tbl`
--

CREATE TABLE `order_tbl` (
  `oid` char(6) NOT NULL,
  `pid` char(6) NOT NULL,
  `qty` int(11) DEFAULT NULL,
  `discount` decimal(5,2) DEFAULT NULL,
  `cid` char(6) DEFAULT NULL,
  `eid` char(6) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `product_tbl`
--

CREATE TABLE `product_tbl` (
  `pid` char(6) NOT NULL,
  `pname` varchar(20) DEFAULT NULL,
  `unitprice` decimal(9,2) DEFAULT NULL,
  `onhand` int(11) DEFAULT NULL,
  `reorder_pt` int(11) DEFAULT NULL,
  `reorder_qty` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `customer_tbl`
--
ALTER TABLE `customer_tbl`
  ADD PRIMARY KEY (`cid`);

--
-- Indexes for table `employee_tbl`
--
ALTER TABLE `employee_tbl`
  ADD PRIMARY KEY (`eid`);

--
-- Indexes for table `order_tbl`
--
ALTER TABLE `order_tbl`
  ADD PRIMARY KEY (`oid`,`pid`),
  ADD KEY `eid` (`eid`),
  ADD KEY `cid` (`cid`),
  ADD KEY `pid` (`pid`);

--
-- Indexes for table `product_tbl`
--
ALTER TABLE `product_tbl`
  ADD PRIMARY KEY (`pid`);

--
-- Constraints for dumped tables
--

--
-- Constraints for table `order_tbl`
--
ALTER TABLE `order_tbl`
  ADD CONSTRAINT `order_tbl_ibfk_1` FOREIGN KEY (`eid`) REFERENCES `employee_tbl` (`eid`) ON UPDATE CASCADE,
  ADD CONSTRAINT `order_tbl_ibfk_2` FOREIGN KEY (`cid`) REFERENCES `customer_tbl` (`cid`) ON UPDATE CASCADE,
  ADD CONSTRAINT `order_tbl_ibfk_3` FOREIGN KEY (`pid`) REFERENCES `product_tbl` (`pid`) ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
