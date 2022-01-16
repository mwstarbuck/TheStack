import React, { Component } from 'react';
import { variables } from './Variables';

export default class Employee extends Component {

  //constructor created below to define state variables
  constructor(props) {
    // The super(props) calls the constructor of its parent class - in this case Departments class
    super(props);

    this.state = {
      departments: [],
      employees: [],
      modalTitle: "",
      EmployeeId: 0,
      EmployeeName: "",
      Department: "",
      DateOfJoining: "",
      PhotoFileName: "anonymous.PNG",
      PhotoPath: variables.PHOTO_URL
    }
    // this.changeDepartmentName = this.changeDepartmentName.bind(this);
  }
  // Gets/Refreshes the Department data from the GetApi method
  refreshList() {
    fetch(variables.API_URL + 'employee')
      .then(response => response.json())
      .then(data => {
        this.setState({ employees: data })
      });

    fetch(variables.API_URL + 'department')
      .then(response => response.json())
      .then(data => {
        this.setState({ departments: data })
      })
  }
  // get departments from the api once the component is mounted
  componentDidMount() {
    this.refreshList();
  }

  changeEmployeeName = (e) => {
    this.setState({
      EmployeeName: e.target.value
    });
  }
  changeDepartment = (e) => {
    this.setState({
      Department: e.target.value
    });
  }
  changeDateOfJoining = (e) => {
    this.setState({
      DateOfJoining: e.target.value
    });
  }

  addClick() {
    this.setState({
      modalTitle: "Add Employee",
      EmployeeId: 0,
      EmployeeName: "",
      Department: "",
      DateOfJoining: "",
      PhotoFileName: "anonymous.png"
    });
  }

  editClick(e) {
    this.setState({
      modalTitle: "Edit Employee",
      EmployeeId: e.EmployeeId,
      EmployeeName: e.EmployeeName,
      Department: e.Department,
      DateOfJoining: e.DateOfJoining,
      PhotoFileName: e.PhotoFileName
    });
  }

  createClick() {
    fetch(variables.API_URL + 'employee', {
      method: 'POST',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({   // [passing new employee info as JSON in the method body
        EmployeeName: this.state.EmployeeName,
        Department: this.state.Department,
        DateOfJoining: this.state.DateOfJoining,
        PhotoFileName: this.state.PhotoFileName
      })
    }).then(res => res.json())
      .then((result) => {
        //display Web API result message
        alert(result);

        //regfresh the dpt list
        this.refreshList();
        // if error, alert user with Failure message
      }, (error) => {
        alert('Failed');
      })
  }

  updateClick() {
    fetch(variables.API_URL + 'employee', {
      method: 'PUT',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({   // passing any updated employee info as JSON in the method body
        EmployeeId: this.state.EmployeeId,
        EmployeeName: this.state.EmployeeName,
        Department: this.state.Department,
        DateOfJoining: this.state.DateOfJoining,
        PhotoFileName: this.state.PhotoFileName
      })
    }).then(res => res.json())
      .then((result) => {
        //display Web API result message
        alert(result);

        //regfresh the dpt list
        this.refreshList();
        // if error, alert user with Failure message
      }, (error) => {
        alert('Failed');
      })
  }

  deleteClick(id) {
    if (window.confirm('Are you sure you want to delete this department?')) {
      fetch(variables.API_URL + 'employee', {
        method: 'DELETE',
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({   // passing new department name as JSON in the method body
          EmployeeId: id
          // DepartmentName: this.state.DepartmentName
        })
      }).then(res => res.json())
        .then((result) => {
          //display Web API result message
          alert(result);

          //regfresh the dpt list
          this.refreshList();
          // if error, alert user with Failure message
        }, (error) => {
          alert('Failed');
        })
    }
  }
  // imageUpload creates form data
  imageUpload = (e) => {
    e.preventDefault();

    const formData = new FormData();
    formData.append("file", e.target.files[0], e.target.files[0].name);

    fetch(variables.API_URL + 'employee/savefile', {
      method: 'POST',
      body: formData
    })
      .then(res => res.json())
      .then(data => {
        this.setState({ PhotoFileName: data });
      })
  }

  render() {
    // declaring state variables inside render() to use in html
    const {
      departments,
      employees,
      modalTitle,
      EmployeeId,
      EmployeeName,
      Department,
      DateOfJoining,
      PhotoFileName,
      PhotoPath
    } = this.state;

    return (
      <div>
        <button type='button'
          className='btn btn-primary m-2 float-end'
          data-bs-toggle='modal'
          data-bs-target='#exampleModal'
          onClick={() => this.addClick()}>
          Add Employee
        </button>
        <h3>This is the Employee page</h3>
        <table className='table table-striped'>
          <thead>
            <tr>
              <th>
                EmployeeId
              </th>
              <th>
                EmployeeName
              </th>
              <th>
                Department
              </th>
              <th>
                DateOfJoining
              </th>
              <th>
                Options
              </th>
            </tr>
          </thead>
          <tbody>
            {employees.map(e =>
              <tr key={e.EmployeeId}>
                <td>{e.EmployeeId}</td>
                <td>{e.EmployeeName}</td>
                <td>{e.Department}</td>
                <td>{e.DateOfJoining}</td>
                <td>
                  <button type="button"
                    className='btn btn-light mr-1'
                    data-bs-toggle='modal'
                    data-bs-target='#exampleModal'
                    onClick={() => this.editClick(e)}>
                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" className="bi bi-pencil-square" viewBox="0 0 16 16">
                      <path d="M15.502 1.94a.5.5 0 0 1 0 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 0 1 .707 0l1.293 1.293zm-1.75 2.456-2-2L4.939 9.21a.5.5 0 0 0-.121.196l-.805 2.414a.25.25 0 0 0 .316.316l2.414-.805a.5.5 0 0 0 .196-.12l6.813-6.814z" />
                      <path fillRule="evenodd" d="M1 13.5A1.5 1.5 0 0 0 2.5 15h11a1.5 1.5 0 0 0 1.5-1.5v-6a.5.5 0 0 0-1 0v6a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11a.5.5 0 0 1 .5-.5H9a.5.5 0 0 0 0-1H2.5A1.5 1.5 0 0 0 1 2.5v11z" />
                    </svg>
                  </button>

                  <button type="button"
                    className='btn btn-light mr-1'
                    onClick={() => this.deleteClick(e.EmployeeId)}>
                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" className="bi bi-trash-fill" viewBox="0 0 16 16">
                      <path d="M2.5 1a1 1 0 0 0-1 1v1a1 1 0 0 0 1 1H3v9a2 2 0 0 0 2 2h6a2 2 0 0 0 2-2V4h.5a1 1 0 0 0 1-1V2a1 1 0 0 0-1-1H10a1 1 0 0 0-1-1H7a1 1 0 0 0-1 1H2.5zm3 4a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 .5-.5zM8 5a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7A.5.5 0 0 1 8 5zm3 .5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 1 0z" />
                    </svg>
                  </button>
                </td>
              </tr>
            )}
          </tbody>
        </table>

        <div className='modal fade' id='exampleModal' tabIndex='-1' aria-hidden="true">
          <div className='modal-dialog modal-lg modal-dialog-centered'>
            <div className='modal-content'>
              <div className='modal-header'>
                <h5 className='modal-title'>{modalTitle}</h5>
                <button type='btn'
                  className='btn-close'
                  data-bs-dismiss="modal"
                  aria-label="Close"
                >
                </button>
              </div>
              <div className='modal-body'>
                <div className='d-flex flex-row bd-highlight mb-3'>

                  <div className='p-2 w-50 bd-highlight'>

                    <div className='input-group mb-3'>
                      <span className='input-group-text'>Name</span>
                      <input type='text' className='form-control'
                        value={EmployeeName}
                        onChange={this.changeEmployeeName} />
                    </div>

                    <div className='input-group mb-3'>
                      <span className='input-group-text'>Department</span>
                      <select className='form-select'
                        onChange={this.changeDepartment}
                        value={Department}>
                        {departments.map(d =>
                          <option key={d.DepartmentId}>
                            {d.DepartmentName}
                          </option>)}
                      </select>
                    </div>

                    <div className='input-group mb-3'>
                      <span className='input-group-text'>DOJ</span>
                      <input type='date' className='form-control'
                        value={DateOfJoining}
                        onChange={this.changeDateOfJoining} />
                    </div>
                  </div>
                  <div className='p-2 w-50 bd-highlight'>
                    <img width="250px" height="250px"
                      src={PhotoPath + PhotoFileName} />
                    <input className='m-2' type="file" onChange={this.imageUpload} />
                  </div>
                </div>
                {EmployeeId == 0 ? <button
                  type='button'
                  className='btn btn-primary float-start'
                  onClick={() => this.createClick()}>
                  Create
                </button>
                  : null}
              </div>
              {EmployeeId != 0 ?
                <button
                  type='button'
                  className='btn btn-primary float-start'
                  onClick={() => this.updateClick()}>
                  Update
                </button>
                : null}
            </div>
          </div>
        </div>
      </div>
    )
  }
}