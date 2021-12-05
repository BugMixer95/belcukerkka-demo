$(document).ready(function () {
	const deadline = new Date('January 1, 2022 00:00:00');

	let timerId = null;

	function declensionNum(num, words) {
		return words[(num % 100 > 4 && num % 100 < 20) ? 2 : [2, 0, 1, 1, 1, 2][(num % 10 < 5) ? num % 10 : 5]];
	}

	function countdownTimer() {
		const diff = deadline - new Date();
		if (diff <= 0) {
			clearInterval(timerId);
		}
		
		const days = diff > 0 ? Math.floor(diff / 1000 / 60 / 60 / 24) : 0;
		const hours = diff > 0 ? Math.floor(diff / 1000 / 60 / 60) % 24 : 0;
		const minutes = diff > 0 ? Math.floor(diff / 1000 / 60) % 60 : 0;
		const seconds = diff > 0 ? Math.floor(diff / 1000) % 60 : 0;
		
		$days.textContent = days < 10 ? '0' + days : days;
		$hours.textContent = hours < 10 ? '0' + hours : hours;
		$minutes.textContent = minutes < 10 ? '0' + minutes : minutes;
		$seconds.textContent = seconds < 10 ? '0' + seconds : seconds;

		$daysWords.textContent = declensionNum(days, ['день', 'дня', 'дней']);
		$hoursWords.textContent = declensionNum(hours, ['час', 'часа', 'часов']);
		$minutesWords.textContent = declensionNum(minutes, ['минута', 'минуты', 'минут']);
		$secondsWords.textContent = declensionNum(seconds, ['секунда', 'секунды', 'секунд']);
	}

	const $days = document.querySelector('.cs-timer__days');
	const $hours = document.querySelector('.cs-timer__hours');
	const $minutes = document.querySelector('.cs-timer__minutes');
	const $seconds = document.querySelector('.cs-timer__seconds');

	const $daysWords = document.querySelector('.cs-timer__days-words');
	const $hoursWords = document.querySelector('.cs-timer__hours-words');
	const $minutesWords = document.querySelector('.cs-timer__minutes-words');
	const $secondsWords = document.querySelector('.cs-timer__seconds-words');

	countdownTimer();

	timerId = setInterval(countdownTimer, 1000);
});