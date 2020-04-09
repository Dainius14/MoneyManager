window.FomanticUIDropdown = {
	preventInputKeyDownDefaults: (element) => {
		element.addEventListener('keydown', (e) => {
			switch (e.key) {
				case "ArrowUp":
				case "ArrowDown":
				case "Enter":
					e.preventDefault();
					break;
			}
		});
	},
	blurElement: (element) => {
		element.blur();
	}
}
