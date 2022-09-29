import { Component, Inject, Input, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { UserReadDto } from '../../../../../Dto/userDto';
import { AdminService } from '../../../services/admin.service';

@Component({
  selector: 'app-account-detail',
  templateUrl: './account-detail.component.html',
  styleUrls: ['./account-detail.component.scss']
})
export class AccountDetailComponent implements OnInit {

  @Input() userData!: UserReadDto;
  $unsubscriber = new Subject();
  avatar?: object

  constructor(
    private _adminService: AdminService,
  ) { }
  ngOnInit(): void {
  }

  onSelectFile(event: any) { // called each time file input changes
    if (event.target.files && event.target.files[0]) {
      var reader = new FileReader();

      reader.readAsDataURL(event.target.files[0]); // read file as data url

      reader.onload = (event: any) => { // called once readAsDataURL is completed
       // this.url = event.target.result;
      }

      let formData= new FormData();
      formData.append('file',event.target.files[0]);
      this._adminService.changeAvatar(this.userData.id,formData).pipe(takeUntil(this.$unsubscriber)).subscribe(
        res => {
         console.log(res);
        },
        res=>{
          console.log(res);

        }

      )

    }
  }


  ngOnDestroy(): void {
    this.$unsubscriber.next();
    this.$unsubscriber.complete();
  }
}

