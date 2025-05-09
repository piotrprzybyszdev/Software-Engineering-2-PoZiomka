import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { MatchService } from './match.service';
import { MatchStatus } from './match.model';
import { ToastrService } from 'ngx-toastr';
import { StudentModel } from '../student.model';
import { StudentService } from '../student.service';
import { LoadingButtonComponent } from "../../common/loading-button/loading-button.component";
import { StudentProfileComponent } from "./student-profile/student-profile.component";
import { AnswersViewComponent } from "./answers-view/answers-view.component";

type Match = {
  id: number,
  otherStudent: StudentModel,
  thisStatus: MatchStatus,
  otherStatus: MatchStatus,
  isAccepted: boolean,
  isRejected: boolean
};

@Component({
  selector: 'app-matches',
  imports: [LoadingButtonComponent, StudentProfileComponent, AnswersViewComponent],
  templateUrl: './matches.component.html',
  styleUrl: './matches.component.css'
})
export class MatchesComponent implements OnInit {
  private studentService = inject(StudentService);
  private matchService = inject(MatchService);
  private toastrService = inject(ToastrService);

  thisStudentId = computed(() => this.studentService.loggedInStudent()!.id);
  matches = signal<Match[]>([]);

  isAccepting = signal<boolean[]>([]);
  isRejecting = signal<boolean[]>([]);

  selectedStudentProfileId = signal<number | undefined>(undefined);
  selectedStudentFormsId = signal<number | undefined>(undefined);

  matchToString(match: Match): string {
    if (match.isAccepted) {
      return "Zaakceptowane";
    }
    if (match.otherStatus === MatchStatus.rejected) {
      return "Odrzucone prez drugą osobę";
    }
    if (match.thisStatus === MatchStatus.rejected) {
      return "Odrzucone przez ciebie";
    }
    if (match.otherStatus === match.thisStatus) {
      return "Niezaakceptowane";
    }
    if (match.thisStatus === MatchStatus.pending) {
      return "Oczekiwanie na Ciebie";
    }
    return "Oczekiwanie na drugą osobę";
  }
  
  matchToColorString(match: Match): string {
    if (match.isAccepted) {
      return "success";
    }
    if (match.isRejected) {
      return "danger";
    }
    if (match.otherStatus === MatchStatus.pending) {
      return "warning";
    }
    return "primary";
  }

  ngOnInit(): void {
    this.loadMatches();
  }

  loadMatches(): void {
    this.matchService.getStudentMatches().subscribe({
      next: response => {
        if (response.success) {
          const matches = response.payload!;

          this.isAccepting.set(Array(matches.length).fill(false));
          this.isRejecting.set(Array(matches.length).fill(false));
          matches.forEach(match => {
            const isOther1 = match.studentId1 !== this.thisStudentId();
            
            this.studentService.getStudent(isOther1 ? match.studentId1 : match.studentId2).subscribe({
              next: response => {
                if (response.success) {
                  const otherStudent = response.payload!;

                  this.matches.update(value => [...value, {
                    id: match.id,
                    otherStudent: otherStudent,
                    thisStatus: isOther1 ? match.status1 : match.status2,
                    otherStatus: isOther1 ? match.status2 : match.status1,
                    isAccepted: match.isAccepted,
                    isRejected: match.isRejected
                  }]);
                } else {
                  this.toastrService.error(response.error!.detail, response.error!.title);
                }
              }
            });            
          });
        } else {
          this.toastrService.error(response.error!.detail, response.error!.title);
        }
      }
    });
  }

  onAccept(index: number): void {
    this.isAccepting.update(value => value.updateClone(index, true));

    this.matchService.updateMatch(this.matches()[index].id, true).subscribe({
      next: response => {
        if (response.success) {
          this.toastrService.success('Pomyślnie zaakceptowano dopasowanie');
          this.loadMatches();
        } else {
          this.toastrService.error(response.error!.detail, response.error!.title);
        }
        this.isAccepting.update(value => value.updateClone(index, false));
      }
    });
  }

  onReject(index: number): void {
    this.isRejecting.update(value => value.updateClone(index, true));

    this.matchService.updateMatch(this.matches()[index].id, false).subscribe({
      next: response => {
        if (response.success) {
          this.toastrService.success('Pomyślnie odrzucono dopasowanie');
          this.loadMatches();
        } else {
          this.toastrService.error(response.error!.detail, response.error!.title);
        }
        this.isRejecting.update(value => value.updateClone(index, false));
      }
    });
  }

  onProfileSelect(studentId: number): void {
    this.selectedStudentProfileId.set(studentId);
  }

  onFormSelect(studentId: number): void {
    this.selectedStudentFormsId.set(studentId);
  }

  onHideProfilePopup(): void {
    this.selectedStudentProfileId.set(undefined);
  }

  onHideFormPopup(): void {
    this.selectedStudentFormsId.set(undefined);
  }
}
