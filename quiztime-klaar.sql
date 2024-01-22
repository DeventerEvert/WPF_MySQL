-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Gegenereerd op: 22 jan 2024 om 14:08
-- Serverversie: 10.4.32-MariaDB
-- PHP-versie: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `quiztime`
--

-- --------------------------------------------------------

--
-- Tabelstructuur voor tabel `answer`
--

CREATE TABLE `answer` (
  `idAnswer` int(11) NOT NULL,
  `answerText` text NOT NULL,
  `image` varchar(255) DEFAULT NULL,
  `isCorrect` tinyint(1) NOT NULL DEFAULT 0,
  `question_idQuestion` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- Gegevens worden geëxporteerd voor tabel `answer`
--

INSERT INTO `answer` (`idAnswer`, `answerText`, `image`, `isCorrect`, `question_idQuestion`) VALUES
(296, 'Hahahahhahahkekekekke', NULL, 1, 96),
(297, 'Wakkekekekekekek', NULL, 0, 96),
(298, 'FaceTheMonster', NULL, 0, 96),
(299, 'I\'veBecome', NULL, 0, 96),
(300, '', 'C:\\Users\\evert\\OneDrive\\Afbeeldingen\\d66fe3e-2da0dd93-106e-42c5-bc99-9c9cde3c700c.png', 0, 97),
(301, '', 'C:\\Users\\evert\\OneDrive\\Afbeeldingen\\fruit.jpg', 1, 97),
(302, '', 'C:\\Users\\evert\\OneDrive\\Afbeeldingen\\zebra.jpg', 0, 97),
(303, '', 'C:\\Users\\evert\\OneDrive\\Afbeeldingen\\quiztime.jpg', 0, 97),
(304, 'Bullets', NULL, 1, 98),
(305, 'Weapons', NULL, 0, 98),
(306, 'Bigger', NULL, 0, 98),
(307, 'Protein', NULL, 0, 98),
(308, '342322', NULL, 0, 99),
(309, '23423', NULL, 0, 99),
(310, '564', NULL, 1, 99),
(311, '4553', NULL, 1, 99),
(312, 'Appiedappie', NULL, 1, 100),
(313, '123', NULL, 0, 100),
(314, '564', NULL, 0, 100),
(315, '324', NULL, 1, 100);

-- --------------------------------------------------------

--
-- Tabelstructuur voor tabel `question`
--

CREATE TABLE `question` (
  `idQuestion` int(11) NOT NULL,
  `questionText` text NOT NULL,
  `image` varchar(255) DEFAULT NULL,
  `idQuiz` int(11) NOT NULL,
  `Type` tinyint(4) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- Gegevens worden geëxporteerd voor tabel `question`
--

INSERT INTO `question` (`idQuestion`, `questionText`, `image`, `idQuiz`, `Type`) VALUES
(96, 'Bwahahahahahhahaha', 'C:\\Users\\evert\\OneDrive\\Afbeeldingen\\fruit.jpg', 40, 0),
(97, 'ABduioasbdiabidsifoto', 'C:\\Users\\evert\\OneDrive\\Afbeeldingen\\fruit.jpg', 40, 0),
(98, 'I need more?', NULL, 41, 0),
(99, 'Hallo ik ben eend', NULL, 40, 1),
(100, 'Hallo ik ben meervoud', 'C:\\Users\\evert\\OneDrive\\Afbeeldingen\\fruit.jpg', 40, 1);

-- --------------------------------------------------------

--
-- Tabelstructuur voor tabel `quiz`
--

CREATE TABLE `quiz` (
  `idQuiz` int(11) NOT NULL,
  `Quizname` varchar(255) DEFAULT NULL,
  `Image` varchar(255) DEFAULT NULL,
  `dateCreated` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- Gegevens worden geëxporteerd voor tabel `quiz`
--

INSERT INTO `quiz` (`idQuiz`, `Quizname`, `Image`, `dateCreated`) VALUES
(40, 'ItsOveerNoow1234', 'file:///C:/Users/evert/OneDrive/Afbeeldingen/zebra.jpg', '2024-01-17 09:58:55'),
(41, 'New Quiz123', 'file:///C:/Users/evert/OneDrive/Afbeeldingen/fruit.jpg', '2024-01-19 11:03:07');

--
-- Indexen voor geëxporteerde tabellen
--

--
-- Indexen voor tabel `answer`
--
ALTER TABLE `answer`
  ADD PRIMARY KEY (`idAnswer`,`question_idQuestion`),
  ADD KEY `fk_answer_question1_idx` (`question_idQuestion`);

--
-- Indexen voor tabel `question`
--
ALTER TABLE `question`
  ADD PRIMARY KEY (`idQuestion`),
  ADD KEY `fk_quiz` (`idQuiz`);

--
-- Indexen voor tabel `quiz`
--
ALTER TABLE `quiz`
  ADD PRIMARY KEY (`idQuiz`);

--
-- AUTO_INCREMENT voor geëxporteerde tabellen
--

--
-- AUTO_INCREMENT voor een tabel `answer`
--
ALTER TABLE `answer`
  MODIFY `idAnswer` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=316;

--
-- AUTO_INCREMENT voor een tabel `question`
--
ALTER TABLE `question`
  MODIFY `idQuestion` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=101;

--
-- AUTO_INCREMENT voor een tabel `quiz`
--
ALTER TABLE `quiz`
  MODIFY `idQuiz` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=42;

--
-- Beperkingen voor geëxporteerde tabellen
--

--
-- Beperkingen voor tabel `answer`
--
ALTER TABLE `answer`
  ADD CONSTRAINT `fk_answer_question1` FOREIGN KEY (`question_idQuestion`) REFERENCES `question` (`idQuestion`) ON DELETE NO ACTION ON UPDATE NO ACTION;

--
-- Beperkingen voor tabel `question`
--
ALTER TABLE `question`
  ADD CONSTRAINT `fk_quiz` FOREIGN KEY (`idQuiz`) REFERENCES `quiz` (`idQuiz`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
