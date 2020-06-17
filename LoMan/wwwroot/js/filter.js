var start = 2019;
var end = new Date().getFullYear();
var options = `<option value="0">Select Year</option>`;
for (var year = start; year <= end; year++) {
	options += `<option value="${year}">${year}</option>`;
}
document.getElementById("year").innerHTML = options
document.getElementById("searchType").addEventListener("change", typeChange);
function typeChange() {
	var type = document.getElementById("searchType");
	if (type.value.indexOf("idate") > -1 || type.value.indexOf("rdate") > -1) {
		document.getElementById("searchInput").setAttribute("type", "date");
	}
	else {
		document.getElementById("searchInput").setAttribute("type", "text");
	}
}

document.getElementById('filterBtn').addEventListener("click", tableFilter)
document.getElementById('searchInput').addEventListener("change", Search)

function Search() {
	let input, filter, table, tr, td, i;
	input = document.getElementById("searchInput").value;
	filter = document.getElementById("searchType").value;
	console.log(filter)
	table = document.getElementById("table-body");
	tr = table.getElementsByTagName("tr");

	// Loop through all table rows, and hide those who don't match the search query
	if (filter === "name") {
		let searchValue = input.toUpperCase();
		for (i = 0; i < tr.length; i++) {
			td = tr[i].getElementsByTagName("td")[0];
			console.log(td);
			if (td) {
				let txtValue = td.textContent || td.innerText;
				if (txtValue.toUpperCase().indexOf(searchValue) > -1) {
					tr[i].style.display = "";
				}
				else {
					tr[i].style.display = "none";
				}
			}
		}
	}
	else if (filter === "idate") {

		let searchValue = new Date(`${input}`);
		let dd1 = searchValue.getDate();
		let mm1 = searchValue.getMonth() + 1;
		//January is 0!
		const yyyy1 = searchValue.getFullYear();
		if (dd1 < 10) {
			dd1 = '0' + dd1
		}
		if (mm1 < 10) {
			mm1 = '0' + mm1
		}
		searchValue = yyyy1 + '-' + mm1 + '-' + dd1;
		for (i = 0; i < tr.length; i++) {
			td = tr[i].getElementsByTagName("td")[1];
			if (td) {
				console.log(td);
				let txtValue = new Date(`${td.innerHTML}`);

				let dd2 = txtValue.getDate();
				let mm2 = txtValue.getMonth() + 1;
				//January is 0!
				const yyyy2 = txtValue.getFullYear();
				if (dd2 < 10) {
					dd2 = '0' + dd2
				}
				if (mm2 < 10) {
					mm2 = '0' + mm2
				}
				txtValue = yyyy2 + '-' + mm2 + '-' + dd2;				
				if (txtValue === searchValue) {				
					tr[i].style.display = "";
				}
				else {
					tr[i].style.display = "none";					
				}
			}
		}
	}
	else if (filter === "rdate") {
		searchValue = input.value;
		for (i = 0; i < tr.length; i++) {
			td = tr[i].getElementsByTagName("td")[2];
			if (td) {
				txtValue = td.value;
				if (txtValue == searchValue) {
					tr[i].style.display = "";
				}
				else {
					tr[i].style.display = "none";
				}
			}
		}
	}
}

function tableFilter() {
	let Month = document.getElementById("month").value;
	let Year = document.getElementById("year").value;
	let table, tr, td, i;
	table = document.getElementById("table-body");
	tr = table.getElementsByTagName("tr");
	if (Month != 0 && Year != 0) {
		for (i = 0; i < tr.length; i++) {
			td = tr[i].getElementsByTagName("td")[1];
			if (td) {
				let iDate = new Date(`${td.innerHTML}`);
				let m = iDate.getMonth() + 1;
				let y = iDate.getFullYear();
				if (Month == m && Year == y) {
					console.log("If is called");
					tr[i].style.display = "";
				}
				else {
					tr[i].style.display = "none";
					console.log("Else  is called");
				}
			}
		}
	}
	else {
		alert("Please Select Year and Month To be Filtered!!");
		for (i = 0; i < tr.length; i++) {
			tr[i].style.display = "";
		}
	}
}