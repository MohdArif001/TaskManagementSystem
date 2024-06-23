var flag = false;
$(document).ready(function () {
    hideLoader();
    bindDropDown(0);
});
function clear() {
    $('#sel1').val('Select');
    $('#sel2').val('Select');
    $('#sel3').val('Select');
    $('#sel4').val('');
    $('#sel5').val('Select');
    $('#sel6').val('Select');
    $('#sel7').val('');
}

$("#addTask").on('click', function () {
    debugger
    let projectId = $('select#sel1 option:selected').val();
    let issueId = $('select#sel2 option:selected').val();
    let statusId = $('select#sel3 option:selected').val();
    let description = $('#sel4').val();
    let priorityId = $('select#sel5 option:selected').val();
    let assignee = $('select#sel6 option:selected').val();
    let taskName = $('#sel7').val();
    if (projectId == "Select" || issueId == "Select" || statusId == "Select" || description == "" || priorityId == "Select" || assignee == "Select" || taskName == "") {
        alert("Please select all fields");
    }
    else {
        showLoader();
        objTaskDetail = {
            TaskName: taskName,
            Description: description,
            StatusId: statusId,
            IssueTypeId: issueId,
            ProjectId: projectId,
            PriorityId: priorityId,
            AssignedTo: assignee,
            CreatedBy: 0,
            ModifiedBy: 0
        };

        $.ajax({
            url: "/Account/CreateTask",
            type: "POST",
            data: objTaskDetail,
            success: function (response) {
                hideLoader();
                alert(response);
                if (response == "Task Created Successfully" || response =="Notification Sent And Task Created Successfully") {
                    $('#createTaskModal').modal('hide');
                    window.location.reload();
                }
            },
            error: function (request, status, error) {
                alert(request.responseText);
            }
        });
    }
   
});
$("#createTask").on("click", function () {
    $("#updateTask").hide();
    $("#addTask").show();
    bindDropDown(1);

});
function getTask(Id) {  
    $("#addTask").hide();
    $("#taskId").val(Id);
    $.ajax({
        url: "/Account/GetTasksById",
        type: "Get",
        data: { id: Id },
        contentType: 'application/json',
        dataType: "JSON",
        async:false,
        success: function (response) {
            debugger
            let taskObject = JSON.parse(response);
            if (taskObject.Message == "Tasks Fetch Successfully") {
                if (taskObject.ResponseData != null) {
                                      
                    $('#sel4').val(taskObject.ResponseData.Description);
                    $('#sel7').val(taskObject.ResponseData.TaskName);
                    setSelectedOption("sel1", taskObject.ResponseData.ProjectId);                 
                    setSelectedOption("sel2", taskObject.ResponseData.IssueTypeId);
                    setSelectedOption("sel3", taskObject.ResponseData.StatusId);
                    setSelectedOption("sel5", taskObject.ResponseData.PriorityId);
                    setSelectedOption("sel6", taskObject.ResponseData.AssignedTo);
                    $("#updateTask").show();
                    $('#createTaskModal').modal('show');
                }

            }
            else {
                alert("No Data Found")
            }
        },
        error: function (request, status, error) {
            alert(request.responseText);
        }
    });
}

function bindDropDown(id) {
    var url = '/Account/GetDropDownList';
    if (id===1) {
        clear();
    }

    if (flag == false) {
        flag = true;
        $.get(url, function (result) {
            if (result != null && result != "") {
                if (result.message == "Data Fetch Successfully") {
                    $.each(result.responseData.projects, function (index, item) {
                        $("#sel1").append("<option value=" + item.id + ">" + item.projectName + "</option>")
                    });
                    $.each(result.responseData.issueTypes, function (index, item) {
                        $("#sel2").append("<option value=" + item.id + ">" + item.issueTypeName + "</option>")
                    });
                    $.each(result.responseData.statuses, function (index, item) {
                        $("#sel3").append("<option value=" + item.id + ">" + item.statusName + "</option>")
                    });
                    $.each(result.responseData.priorities, function (index, item) {
                        $("#sel5").append("<option value=" + item.id + ">" + item.priorityName + "</option>")
                    });
                    $.each(result.responseData.assignees, function (index, item) {
                        $("#sel6").append("<option value=" + item.id + ">" + item.userName + "</option>")
                    });
                }

            }
        });
    }
}

// Function to compare and set the selected option
function setSelectedOption(selectId, valueToCompare) {
    debugger
    // Get the select element by its ID
    var selectElement = document.getElementById(selectId);

    // Iterate over each option in the select element
    for (var i = 0; i < selectElement.options.length; i++) {
        // Get the current option's value
        var optionValue = selectElement.options[i].value;

        // Compare the option's value with the value to compare
        if (optionValue == valueToCompare) {
            // If they match, set the option as selected
            selectElement.options[i].selected = true;
            break // No need to look further
        }
    }
}
$("#updateTask").on("click", function () {
    debugger
    let taskId = $('#taskId').val();
    let projectId = $('select#sel1 option:selected').val();
    let issueId = $('select#sel2 option:selected').val();
    let statusId = $('select#sel3 option:selected').val();
    let description = $('#sel4').val();
    let priorityId = $('select#sel5 option:selected').val();
    let assignee = $('select#sel6 option:selected').val();
    let taskName = $('#sel7').val();
    if (projectId == "Select" || issueId == "Select" || statusId == "Select" || description == "" || priorityId == "Select" || assignee == "Select" || taskName == "") {
        alert("Please select all fields");
    } else {
        showLoader();
        objTaskDetail = {
            Id: taskId,
            TaskName: taskName,
            Description: description,
            StatusId: statusId,
            IssueTypeId: issueId,
            ProjectId: projectId,
            PriorityId: priorityId,
            AssignedTo: assignee,
            CreatedBy: 0,
            ModifiedBy: 0
        };
        $.ajax({
            url: "/Account/UpdateTask",
            type: "POST",
            data: objTaskDetail,
            success: function (response) {
                hideLoader();
                alert(response);
                if (response == "Task Updated Successfully" || response =="Notification Sent And Task Updated Successfully") {
                    $('#createTaskModal').modal('hide');
                    window.location.reload();
                }
            },
            error: function (request, status, error) {
                alert(request.responseText);
            }
        });
    }
});
function deleteTask(Id) {
    debugger
    let result = confirm('Do you Want to Delete');
    if (result) {
        $.ajax({
            url: "/Account/DeleteTasksById",
            type: "Get",
            data: { id: Id },
            contentType: 'application/json',
            dataType: "JSON",
            success: function (response) {
                debugger
                try {
                    let taskObject = JSON.parse(response);
                    if (taskObject.Message == "Tasks Deleted Successfully") {

                        alert("Tasks Deleted Successfully")
                        window.location.reload();
                    }
                    else {
                        alert("Task Not Deleted")
                    }
                }
                catch (err) {
                    alert("Task Not Deleted")
                }

            },
            error: function (request, status, error) {
                alert(request.responseText);
            }
        });
    }
   
}