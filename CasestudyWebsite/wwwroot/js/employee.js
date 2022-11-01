$(() => { // Employeeaddupdate.js
    const getAll = async (msg) => {
        try {
            $("#EmployeeList").text("Finding Employee Information...");
            let response = await fetch(`api/Employee`);
            if (response.ok) {
                let payload = await response.json(); // this returns a promise, so we await it
                buildEmployeeList(payload);
                msg === "" ? // are we appending to an existing message
                    $("#status").text("Employees Loaded") : $("#status").text(`${msg} - Employees Loaded`);
            } else if (response.status !== 404) { // probably some other client side error
                let problemJson = await response.json();
                errorRtn(problemJson, response.status);
            } else { // else 404 not found
                $("#status").text("no such path on server");
            } // else

            // get division data
   
            response = await fetch(`api/Department`);
            if (response.ok) {
                let deps = await response.json(); // this returns a promise, so we await it
                sessionStorage.setItem("alldepartments", JSON.stringify(deps));
            } else if (response.status !== 404) { // probably some other client side error
                let problemJson = await response.json();
                errorRtn(problemJson, response.status);
            } else { // else 404 not found
                $("#status").text("no such path on server");
            } // else
        } catch (error) {
            $("#status").text(error.message);
        }

    }; // getAll
    const setupForUpdate = (id, data) => {
        $("#actionbutton").val("update");
        $("#modaltitle").html("<h4>update Employee</h4>");
        $('#deletealert').hide();
        $('#deleteprompt').show();
        clearModalFields();
        data.forEach(Employee => {
            if (Employee.id === parseInt(id)) {
                $("#TextBoxTitle").val(Employee.title);
                $("#TextBoxFirstname").val(Employee.firstname);
                $("#TextBoxLastname").val(Employee.lastname);
                $("#TextBoxPhone").val(Employee.phoneno);
                $("#TextBoxEmail").val(Employee.email);
                loadDepartmentDDL(Employee.departmentId);
                sessionStorage.setItem("id", Employee.id);
                sessionStorage.setItem("departmentId", Employee.departmentId);
                sessionStorage.setItem("timer", Employee.timer);
                $("#modalstatus").text("update data");
                $("#myModal").modal("toggle");
                $("#myModalLabel").text("Update");
                
            } // if
        }); // data.forEach
    }; // setupForUpdate
    const setupForAdd = () => {
        $("#actionbutton").val("add");
        $("#modaltitle").html("<h4>add Employee</h4>");
        $("#theModal").modal("toggle");
        $("#modalstatus").text("add new Employee");
        $("#myModalLabel").text("Add");
        $('#deletealert').hide();
        $('#deleteprompt').hide();
        clearModalFields();
        
    }; // setupForAdd
    const clearModalFields = () => {
        $("#TextBoxTitle").val("");
        $("#TextBoxFirstname").val("");
        $("#TextBoxLastname").val("");
        $("#TextBoxPhone").val("");
        $("#TextBoxEmail").val("");
        sessionStorage.removeItem("id");
        sessionStorage.removeItem("departmentId");
        sessionStorage.removeItem("timer");
        loadDepartmentDDL(-1);
        $("#myModal").modal("toggle");
    }; // clearModalFields
    const add = async () => {
        try {
            emp = new Object();
            emp.title = $("#TextBoxTitle").val();
            emp.firstname = $("#TextBoxFirstname").val();
            emp.lastname = $("#TextBoxLastname").val();
            emp.phoneno = $("#TextBoxPhone").val();
            emp.email = $("#TextBoxEmail").val();
            emp.departmentId = parseInt($("#ddlDepartment").val());
            emp.id = -1;
            emp.timer = null;
            emp.staffpicture64 = null;
            // send the Employee info to the server asynchronously using POST
            let response = await fetch("api/Employee", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json; charset=utf-8"
                },
                body: JSON.stringify(emp)
            });
            if (response.ok) // or check for response.status
            {
                let data = await response.json();
                getAll(data.msg);
            } else if (response.status !== 404) { // probably some other client side error
                let problemJson = await response.json();
                errorRtn(problemJson, response.status);
            } else { // else 404 not found
                $("#status").text("no such path on server");
            } // else
        } catch (error) {
            $("#status").text(error.message);
        } // try/catch
        $("#myModal").modal("toggle");
    }; // add


    const _delete = async () => {
        try {
            let response = await fetch(`api/Employee/${sessionStorage.getItem('id')}`, {
                method: 'DELETE',
                headers: { 'Content-Type': 'application/json; charset=utf-8' }
            });
            if (response.ok) // or check for response.status
            {
                let data = await response.json();
                getAll(data.msg);
            } else {
                $('#status').text(`Status - ${response.status}, Problem on delete server side, see server console`);
            } // else
            $('#myModal').modal('toggle');
        } catch (error) {
            $('#status').text(error.message);
        }
    };


    const update = async () => {
        try {
            // set up a new client side instance of Employee
            emp = new Object();
            // pouplate the properties
            emp.title = $("#TextBoxTitle").val();
            emp.firstname = $("#TextBoxFirstname").val();
            emp.lastname = $("#TextBoxLastname").val();
            emp.phoneno = $("#TextBoxPhone").val();
            emp.email = $("#TextBoxEmail").val();
            // we stored these 3 earlier
            emp.id = parseInt(sessionStorage.getItem("id"));
            emp.departmentId = parseInt($("#ddlDepartment").val());
            emp.timer = sessionStorage.getItem("timer");
            emp.staffpicture64 = null;
            // send the updated back to the server asynchronously using PUT
            let response = await fetch("api/Employee", {
                method: "PUT",
                headers: { "Content-Type": "application/json; charset=utf-8" },
                body: JSON.stringify(emp)
            });
            if (response.ok) // or check for response.status
            {
                let data = await response.json();
                getAll(data.msg);
            } else if (response.status !== 404) { // probably some other client side error
                let problemJson = await response.json();
                errorRtn(problemJson, response.status);
            } else { // else 404 not found
                $("#status").text("no such path on server");
            } // else
        } catch (error) {
            $("#status").text(error.message);
        } // try/catch
        $("#myModal").modal("toggle");
    } // update
    $("#actionbutton").click(() => {
        $("#actionbutton").val() === "update" ? update() : add();
    });

    $('#deleteprompt').click((e) => {
        $('#deletealert').show();
    });
    $('#deletenobutton').click((e) => {
        $('#deletealert').hide();
    });
    $('#deletebutton').click(() => {
        _delete();
    });

    $("#EmployeeList").click((e) => {
        if (!e) e = window.event;
        let id = e.target.parentNode.id;
        if (id === "EmployeeList" || id === "") {
            id = e.target.id;
        } // clicked on row somewhere else
        if (id !== "status" && id !== "heading") {
            let data = JSON.parse(sessionStorage.getItem("allEmployees"));
            id === "0" ? setupForAdd() : setupForUpdate(id, data);
        } else {
            return false; // ignore if they clicked on heading or status
        }
    }); // EmployeeList click


    const buildEmployeeList = (data) => {
        $("#EmployeeList").empty();
        div = $(`<div class="list-group-item" id="status">Employee Info</div>
<div class= "list-group-item row d-flex text-center" id="heading">
<div class="col-4 h4">Title</div>
<div class="col-4 h4">First</div>
<div class="col-4 h4">Last</div>
</div>`);
        div.appendTo($("#EmployeeList"));
        sessionStorage.setItem("allEmployees", JSON.stringify(data));
        btn = $(`<button class="list-group-item row d-flex" id="0">...click to add Employee</button>`);
        btn.appendTo($("#EmployeeList"));
        data.forEach((emp) => {
            btn = $(`<button class="list-group-item row d-flex" id="${emp.id}">`);
            btn.html(`<div class="col-4" id="Employeetitle${emp.id}">${emp.title}</div>
<div class="col-4" id="Employeefname${emp.id}">${emp.firstname}</div>
<div class="col-4" id="Employeelastnam${emp.id}">${emp.lastname}</div>`
            );
            btn.appendTo($("#EmployeeList"));
        }); // forEach
    }; // buildEmployeeList

    const loadDepartmentDDL = (empdep) => {
        html = '';
        $('#ddlDepartment').empty();
        let alldepartments = JSON.parse(sessionStorage.getItem('alldepartments'));
        alldepartments.forEach((dep) => { html += `<option value="${dep.id}">${dep.name}</option>` });
        $('#ddlDepartment').append(html);
        $('#ddlDepartment').val(empdep);
    }; // loadDepartmentDDL

    getAll(""); // first grab the data from the server
}); // jQuery ready method
// server was reached but server had a problem with the call
const errorRtn = (problemJson, status) => {
    if (status > 499) {
        $("#status").text("Problem server side, see debug console");
    } else {
        let keys = Object.keys(problemJson.errors)
        problem = {
            status: status,
            statusText: problemJson.errors[keys[0]][0], // first error
        };
        $("#status").text("Problem client side, see browser console");
        console.log(problem);
    } // else
}