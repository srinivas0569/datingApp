import { HttpClient } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'The Dating App';
  @ViewChild('userPopup') userPopup;
  users: any;
  form: FormGroup;
  modalRef?: BsModalRef;
  constructor(private http: HttpClient, private _fb: FormBuilder, private modalService: BsModalService) {

  }
  ngOnInit() {
    this.getUsers();
    this.initUserForm();
  }
  initUserForm() {
    this.form = this._fb.group({
      id: 0,
      userName: ''
    })
  }
  getUsers() {
    this.http.get('https://localhost:5001/api/Users').subscribe((response) => {
      this.users = response
    }, error => {
      console.log(error)
    }
    )
  }
  createPopup(){
    this.modalRef = this.modalService.show(this.userPopup);
    this.initUserForm()
  }
  createUser() {
    this.http.post("https://localhost:5001/api/User/UserData", this.form.value).subscribe((response) => {
      console.log(response);
      this.initUserForm()
      this.modalRef.hide();
      this.getUsers()
    }, error => {
      console.log(error)
    }
    )
  }
  openEditPopup(id) {
    this.http.get("https://localhost:5001/api/User/UserData/" + id).subscribe((response) => {
      this.modalRef = this.modalService.show(this.userPopup);
      this.fillFormValue(response)
    }, error => {
      console.log(error)
    }
    )
  }
  editUser(form){
    this.http.put("https://localhost:5001/api/User/UserData", form.value).subscribe((response) => {
      this.getUsers();
      this.modalRef.hide();
    }, error => {
      console.log(error)
    }
    )
  }
  deleteUser(id) {
    this.http.delete("https://localhost:5001/api/User/UserData?id=" + id).subscribe((response) => {
      console.log(response);
      this.getUsers()
    }, error => {
      console.log(error)
    }
    )
  }
  fillFormValue(response) {
    this.initUserForm();
    this.form.patchValue({
      id: response.id,
      userName: response.userName
    })
  }
}
